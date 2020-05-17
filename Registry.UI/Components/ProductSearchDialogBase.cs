using DevExpress.Blazor;
using DevExpress.Blazor.Internal;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Data.ResponseModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
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
        public string IconClass { get; set; } = "fas fa-plus-circle";

        public bool UserHasUpdated { get; set; } = false;

        // TODO: Add user table after implementing auth
        public const int UserId = 1;
        public List<ProductAggregate> Products { get; set; } = new List<ProductAggregate>();
        public int Count { get; set; }
        public bool PopupVisible { get; set; } = false;
        public PagedResult<Models.Review> PagedReviews { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await LoadProducts(new LoadDataArgs()
            {
                Skip = 0,
                Top = InitialProductPageSize
            });
        }
        public async Task LoadProducts(LoadDataArgs args)
        {
            try
            {
                var skip = args.Skip.GetValueOrDefault();
                currentPageIndex = (args.Skip.GetValueOrDefault() / args.Top.GetValueOrDefault()) + 1;
                currentPageSize = args.Top.GetValueOrDefault();
                PagedResult<Models.Product> pagedResult;
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    pagedResult = await ProductRepository.SearchProducts(SearchText, currentPageIndex, currentPageSize);
                }
                else
                {
                    pagedResult = await ProductRepository.GetAllProducts(currentPageIndex, currentPageSize);
                }

                var registryForUser = await RegistryRepository.GetRegistryForUser(UserId);
                if (registryForUser != null && registryForUser.Count > 0)
                {
                    Products = pagedResult.Data.Select(p => p.ToAggregate(registryForUser.FirstOrDefault(r => r.ProductId == p.Id), registryForUser.Any(r => r.ProductId == p.Id))).ToList();
                }
                else
                {
                    Products = pagedResult.Data.Select(p => p.ToAggregate(null, false)).ToList();
                }

                Count = pagedResult.TotalCount;
                StateHasChanged();
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
            CloseEventCallBack.InvokeAsync(UserHasUpdated);
            StateHasChanged();
        }

        public async void Search(MouseEventArgs args)
        {
            await LoadProducts(new LoadDataArgs() { Skip = 0, Top = InitialProductPageSize });
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
            await LoadProducts(new LoadDataArgs() { Skip = 0, Top = InitialProductPageSize });
            StateHasChanged();
        }

        public async Task OnRowExpanded(Models.ProductAggregate product)
        {
            PagedReviews = new PagedResult<Models.Review>();
            try
            {
                var result = await ReviewRepository.GetReviewsForProduct(product.Id, 1, InitialReviewPageSize);
                PagedReviews.Data = result.Data;
                PagedReviews.PageIndex = result.PageIndex;
                PagedReviews.TotalCount = result.TotalCount;
                PagedReviews.TotalPages = result.TotalPages;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
