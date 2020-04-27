using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using Registry.Models;
using Registry.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Registry.UI.Components
{
    public class ProductSearchDialogBase: ComponentBase
    {
        [Inject]
        public IProductRepository ProductRepository { get; set; }
        
        [Parameter]
        public EventCallback<bool> CloseEventCallBack { get; set; }
        public bool ShowDialog { get; set; }

        public string SearchText { get; set; }
        public PagedResult<Models.Product> Products { get; set; } = new PagedResult<Models.Product>();
        public const int InitialPageIndex = 1;
        public const int InitialPageSize = 3;

        public async Task<LoadResult> LoadProducts(DataSourceLoadOptionsBase options, CancellationToken cancellationToken)
        {
            var result = new LoadResult();
            var pagedResult = await ProductRepository.GetAllProducts((options.Skip/options.Take) + 1, options.Take);
            result.totalCount = pagedResult.TotalCount;
            result.data = pagedResult.Data;

            return result;
        }
        public void Show()
        {
            // ResetDialog();
            ShowDialog = true;
            StateHasChanged();
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }
    }
}
