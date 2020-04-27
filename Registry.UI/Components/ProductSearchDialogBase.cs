using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Registry.Models;
using Registry.Repository;
using Registry.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Registry.UI.Components
{
    public class ProductSearchDialogBase : ComponentBase
    {
        [Inject]
        public IProductRepository ProductRepository { get; set; }

        [Inject]
        public IReviewRepository ReviewRepository { get; set; }

        [Inject]
        public IRegistryRepository RegistryRepository { get; set; }

        [Parameter]
        public EventCallback<bool> CloseEventCallBack { get; set; }
        public bool ShowDialog { get; set; }
        public string SearchText { get; set; }
        public const int InitialProductPageSize = 3;
        public const int InitialReviewPageSize = 3;
        public int currentPageIndex { get; set; }
        public int currentPageSize { get; set; }
        public DxDataGrid<ProductAggregate> _gridRef { get; set; }
        public string IconClass { get; set; } = "fas fa-plus-circle";

        public bool UserHasUpdated { get; set; } = false;

        // TODO: Add user table after implementing auth
        public const int UserId = 1;

        public bool PopupVisible { get; set; } = false;
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
                var registryForUser = await RegistryRepository.GetRegistryForUser(UserId);
                if (registryForUser != null && registryForUser.Count > 0)
                {
                    result.data = pagedResult.Data.Select(p => p.ToAggregate(registryForUser.FirstOrDefault(r => r.ProductId == p.Id), registryForUser.Any(r => r.ProductId == p.Id)));
                }
                else
                {
                    result.data = pagedResult.Data.Select(p => p.ToAggregate(null, false));
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public async void Update(Models.ProductAggregate product)
        //{
        //    try
        //    {
        //        var result = Task.Run(() => { return RegistryRepository.UpdateRegistry(new RegistryRecord() { UserId = userId, Quantity = product.Quantity, ProductId = product.Id, Id = product.RegistryId??0 }); }).Result;
        //        await _gridRef.Refresh();
        //        StateHasChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
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
            CloseEventCallBack.InvokeAsync(UserHasUpdated);
            StateHasChanged();
        }

        public async void Search(MouseEventArgs args)
        {
            await _gridRef.Refresh();
        }

        public void ShowValidationError()
        {
            PopupVisible = true;
            StateHasChanged();
        }

        public async void RefreshData()
        {
            PopupVisible = false;
            UserHasUpdated = true;
            await _gridRef.Refresh();
            StateHasChanged();
        }

        //public async void AddOrRemove(Models.ProductAggregate product)
        //{
        //    if (product.Quantity != null && product.Quantity > 0)
        //    {
        //        PopupVisible = false;
        //        UserHasUpdated = true;
        //        if (product.IsAdded)
        //        {
        //            await RegistryRepository.RemoveFromRegistry(product.RegistryId);
        //        }
        //        else
        //        {
        //            await RegistryRepository.AddToRegistry(new RegistryRecord() { ProductId = product.Id, Quantity = product.Quantity, UserId = 1 });
        //        }
        //        await _gridRef.Refresh();
        //        StateHasChanged();
        //    }
        //    else
        //    {
        //        PopupVisible = true;
        //        StateHasChanged();
        //    }
        //}

        public PagedResult<Models.Review> OnRowExpanded(Models.ProductAggregate product)
        {
            var pagedResults = new PagedResult<Models.Review>();
            try
            {
                if (_gridRef.DetailRows.IsRowExpanded(product))
                {
                    var result = Task.Run(() => { return ReviewRepository.GetReviewsForProduct(product.Id, 1, InitialReviewPageSize); }).Result;
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
