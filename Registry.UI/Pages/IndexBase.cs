using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevExpress.Blazor;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Registry.Models;
using Registry.Repository;
using Registry.UI.Components;
using Registry.UI.Extensions;

namespace Registry.UI.Pages
{
    public class IndexBase: ComponentBase
    {
        [Inject]
        public IRegistryRepository RegistryRepository { get; set; }

        [Inject]
        public IProductRepository ProductRepository { get; set; }

        public DxDataGrid<ProductAggregate> _gridRef { get; set; }

        protected ProductSearchDialog ProductSearchDialog { get; set; }

        public List<ProductAggregate> Products { get; set; } = new List<ProductAggregate>();

        public List<RegistryRecord> RegistryRecords { get; set; } = new List<RegistryRecord>();
        public bool IsEmptyRegistry { get; set; }

        public bool PopupVisible { get; set; } = false;

        // TODO: Create a user table after implementing basic auth
        public const int UserId = 1;

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            RegistryRecords = await RegistryRepository.GetRegistryForUser(UserId);
            IsEmptyRegistry = RegistryRecords == null;

            if (!IsEmptyRegistry)
            {
                var products = await ProductRepository.GetProductsByIds(RegistryRecords.Select(r => r.ProductId).ToList());
                Products = products.Select(p => p.ToAggregate(RegistryRecords.FirstOrDefault(r => r.ProductId == p.Id), true)).ToList();
            }
        }
        public void ShowValidationError()
        {
            PopupVisible = true;
            StateHasChanged();
        }

        public async void RefreshData()
        {
            PopupVisible = false;
            await LoadData();
            StateHasChanged();
        }

        public void Search(MouseEventArgs args)
        {
            ProductSearchDialog.Show();
        }

        public async void SearchProductsDialog_OnDialogClose()
        {
            await LoadData();
            StateHasChanged();
        }

    }
}
