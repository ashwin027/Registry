﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Reviews.Repository;

namespace Reviews.Repository.Migrations
{
    [DbContext(typeof(ReviewContext))]
    partial class ReviewContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Reviews.Models.Entities.Review", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<DateTime>("SubmitDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Reviews");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "Great Product!",
                            ProductId = 1,
                            Rating = 5,
                            SubmitDate = new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Local),
                            Title = "Perfect!"
                        },
                        new
                        {
                            Id = 2,
                            Description = "This is a great pacifier.",
                            ProductId = 2,
                            Rating = 4,
                            SubmitDate = new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Local),
                            Title = "Good product!"
                        },
                        new
                        {
                            Id = 3,
                            Description = "The cube started to break after the first week of use.",
                            ProductId = 3,
                            Rating = 1,
                            SubmitDate = new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Local),
                            Title = "Terrible product!"
                        },
                        new
                        {
                            Id = 4,
                            Description = "This teether worked well for my toddler.",
                            ProductId = 1,
                            Rating = 3,
                            SubmitDate = new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Local),
                            Title = "Ok product"
                        },
                        new
                        {
                            Id = 5,
                            Description = "This is a decent product and worked well.",
                            ProductId = 2,
                            Rating = 5,
                            SubmitDate = new DateTime(2020, 4, 26, 0, 0, 0, 0, DateTimeKind.Local),
                            Title = "Decent pacifier."
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
