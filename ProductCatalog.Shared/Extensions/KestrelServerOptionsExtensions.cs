﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ProductCatalog.Shared.Extensions
{
    public static class KestrelServerOptionsExtensions
    {
        public static void ConfigureEndpoints(this KestrelServerOptions options)
        {
            var configuration = options.ApplicationServices.GetRequiredService<IConfiguration>();
            var environment = options.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var httpEndpointConfig = new EndpointConfiguration();
            var httpsEndpointConfig = new EndpointConfiguration();

            configuration.GetSection("HttpServer:Http").Bind(httpEndpointConfig);
            configuration.GetSection("HttpServer:Https").Bind(httpsEndpointConfig);

            options.AddListener(httpEndpointConfig, environment);
            options.AddListener(httpsEndpointConfig, environment);
        }

        public static void ConfigureGRPCEndpoints(this KestrelServerOptions options)
        {
            var configuration = options.ApplicationServices.GetRequiredService<IConfiguration>();
            var environment = options.ApplicationServices.GetRequiredService<IWebHostEnvironment>();
            var http2EndpointConfig = new EndpointConfiguration();

            configuration.GetSection("HttpServer:Https2").Bind(http2EndpointConfig);

            options.AddGRPCListener(http2EndpointConfig, environment);
        }

        private static void AddListener(this KestrelServerOptions options, EndpointConfiguration config, IWebHostEnvironment environment)
        {
            var port = config.Port ?? (config.Scheme == "https" ? 443 : 80);

            var ipAddresses = new List<IPAddress>();
            if (config.Host == "localhost")
            {
                ipAddresses.Add(IPAddress.IPv6Loopback);
                ipAddresses.Add(IPAddress.Loopback);
            }
            else if (IPAddress.TryParse(config.Host, out var address))
            {
                ipAddresses.Add(address);
            }
            else
            {
                ipAddresses.Add(IPAddress.IPv6Any);
            }

            foreach (var address in ipAddresses)
            {
                options.Listen(address, port,
                    listenOptions =>
                    {
                        if (config.Scheme == "https")
                        {
                            var certificate = LoadCertificate(config, environment);
                            listenOptions.UseHttps(certificate);
                        }
                    });
            }
        }

        private static void AddGRPCListener(this KestrelServerOptions options, EndpointConfiguration config, IWebHostEnvironment environment)
        {
            var port = config.Port ?? 443;
            if (IPAddress.TryParse(config.Host, out var address))
            {
                options.Listen(address, port, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    listenOptions.UseHttps(LoadCertificate(config, environment));
                });
            }
            else
            {
                options.Listen(IPAddress.Any, port, listenOptions =>
                {
                    listenOptions.Protocols = HttpProtocols.Http2;
                    listenOptions.UseHttps(LoadCertificate(config, environment));
                });
            }
        }

        public static X509Certificate2 LoadCertificate(EndpointConfiguration config, IWebHostEnvironment environment)
        {
            if (config.StoreName != null && config.StoreLocation != null)
            {
                using (var store = new X509Store(config.StoreName, Enum.Parse<StoreLocation>(config.StoreLocation)))
                {
                    store.Open(OpenFlags.ReadOnly);
                    var certificate = store.Certificates.Find(
                        X509FindType.FindBySubjectName,
                        config.Host,
                        validOnly: !environment.IsDevelopment());

                    if (certificate.Count == 0)
                    {
                        throw new InvalidOperationException($"Certificate not found for {config.Host}.");
                    }

                    return certificate[0];
                }
            }

            if (config.FilePath != null && config.Password != null)
            {
                return new X509Certificate2(config.FilePath, config.Password);
            }

            throw new InvalidOperationException("No valid certificate configuration found for the current endpoint.");
        }
    }
}
