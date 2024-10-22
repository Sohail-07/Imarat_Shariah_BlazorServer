using Imarat_Shariah.Data.Entities;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class SiyajatForm
    {
        [Parameter]
        public Siyajat SiyajatModel { get; set; } = new Siyajat();

        [Parameter]
        public EventCallback<Siyajat> OnSubmit { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public bool IsEditMode { get; set; }

        private async Task HandleValidSubmit()
        {
            await OnSubmit.InvokeAsync(SiyajatModel);
        }

        private async Task Cancel()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
