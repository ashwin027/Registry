using Microsoft.AspNetCore.Components;
using Registry.Models;
using Registry.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registry.UI.Components
{
    public class RegistryActionsBase: ComponentBase
    {
        [Parameter]
        public Models.ProductAggregate ProductAggregate { get; set; }

        [Inject]
        public IRegistryRepository RegistryRepository { get; set; }

        [Parameter]
        public EventCallback<bool> OnValidationError { get; set; }

        [Parameter]
        public EventCallback<bool> OnUpdated { get; set; }

        [Parameter]
        public int UserId { get; set; } 
        public async void AddOrRemove(Models.ProductAggregate product)
        {
            try
            {
                if (product.Quantity != null && product.Quantity > 0)
                {
                    if (product.IsAdded)
                    {
                        await RegistryRepository.RemoveFromRegistry(product.RegistryId);
                    }
                    else
                    {
                        await RegistryRepository.AddToRegistry(new RegistryRecord() { ProductId = product.Id, Quantity = product.Quantity, UserId = 1 });
                    }
                    await OnUpdated.InvokeAsync(true);
                    StateHasChanged();
                }
                else
                {
                    await OnValidationError.InvokeAsync(true);
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async void Update(Models.ProductAggregate product)
        {
            try
            {
                var result = Task.Run(() => { return RegistryRepository.UpdateRegistry(new RegistryRecord() { UserId = UserId, Quantity = product.Quantity, ProductId = product.Id, Id = product.RegistryId ?? 0 }); }).Result;
                await OnUpdated.InvokeAsync(true);
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
