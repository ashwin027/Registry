using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Registry.Models;
using Registry.Repository;
using Registry.UI.Extensions;

namespace Registry.UI.Pages
{
    public class IndexBase: ComponentBase
    {
        [Inject]
        public IRegistryRepository _registryRepository { get; set; }

        [Inject]
        public IProductRepository _productRepository { get; set; }

        public List<ProductAggregate> Products { get; set; } = new List<ProductAggregate>();

        public List<RegistryRecord> RegistryRecords { get; set; } = new List<RegistryRecord>();
        public bool IsEmptyRegistry { get; set; }

        // TODO: Create a user table after implementing basic auth
        private const int userId = 1;

        protected override async Task OnInitializedAsync()
        {
            RegistryRecords = await _registryRepository.GetRegistryForUser(userId);
            IsEmptyRegistry = RegistryRecords == null;

            if (!IsEmptyRegistry)
            {
                var products = await _productRepository.GetProductsByIds(RegistryRecords.Select(r => r.ProductId).ToList());
                Products = products.Select(p => p.ToAggregate(RegistryRecords.FirstOrDefault(r => r.ProductId == p.Id)?.Quantity)).ToList();
            }
        }

        public void Search(MouseEventArgs args)
        {

        }
    }
}
