using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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

        [Inject]
        public IReviewRepository ReviewRepository { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallBack { get; set; }
        public bool ShowDialog { get; set; }
        public string SearchText { get; set; }
        public const int InitialPageSize = 3;
        public int currentPageIndex { get; set; }
        public int currentPageSize { get; set; }
        public DxDataGrid<Product> _gridRef { get; set; }
        public async Task<LoadResult> LoadProducts(DataSourceLoadOptionsBase options, CancellationToken cancellationToken)
        {
            try
            {
                var result = new LoadResult();
                currentPageIndex = (options.Skip / options.Take) + 1;
                currentPageSize = options.Take;
                PagedResult<Models.Product> pagedResult;
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    pagedResult = await ProductRepository.SearchProducts(SearchText, currentPageIndex, currentPageSize);
                }
                else
                {
                    pagedResult = await ProductRepository.GetAllProducts(currentPageIndex, currentPageSize);
                }
                
                result.totalCount = pagedResult.TotalCount;
                result.data = pagedResult.Data;

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Show()
        {
            ResetDialog();
            ShowDialog = true;
            StateHasChanged();
        }

        public void ResetDialog()
        {
            SearchText = string.Empty;
            currentPageIndex = default;
            currentPageSize = default;
        }

        public void Close()
        {
            ShowDialog = false;
            StateHasChanged();
        }

        public async void Search(MouseEventArgs args)
        {
            await _gridRef.Refresh();
        }

        public PagedResult<Models.Review> OnRowExpanded(Models.Product product)
        {
            var pagedResults = new PagedResult<Models.Review>();
            try
            {
                if (_gridRef.DetailRows.IsRowExpanded(product))
                {
                    var result = Task.Run(() => { return ReviewRepository.GetReviewsForProduct(product.Id, 1, InitialPageSize); }).Result;
                    pagedResults.Data = result.Data;
                    pagedResults.PageIndex = result.PageIndex;
                    pagedResults.TotalCount = result.TotalCount;
                    pagedResults.TotalPages = result.TotalPages;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return pagedResults;
        }
    }
}
