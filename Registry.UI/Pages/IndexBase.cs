using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Registry.Models;
using Registry.Repository;

namespace Registry.UI.Pages
{
    public class IndexBase: ComponentBase
    {
        [Inject]
        public IRegistryRepository _registryRepository { get; set; }

        [Inject]
        public IProductRepository _productRepository { get; set; }

        List<Product> Products { get; set; } = new List<Product>();

        List<RegistryRecord> RegistryRecords { get; set; } = new List<RegistryRecord>();
        public bool IsEmptyRegistry { get; set; }

        // TODO: Create a user table with auth
        private const int userId = 1;

        protected override async Task OnInitializedAsync()
        {
            RegistryRecords = await _registryRepository.GetRegistryForUser(userId);
            IsEmptyRegistry = RegistryRecords == null;


        }
    }
}
