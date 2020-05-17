using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Registry.UI.Components
{
    public class SimpleDialogBase: ComponentBase
    {
        [Inject]
        public DialogService DialogService { get; set; }

        [Parameter] 
        public string Title { get; set; }

        [Parameter]
        public string Message { get; set; }
        public void Close()
        {
            DialogService.Close();
            StateHasChanged();
        }
    }
}
