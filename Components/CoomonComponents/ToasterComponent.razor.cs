using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.CoomonComponents
{
    public class ToasterBase : ComponentBase
    {
        [Parameter] public string AlertType { get; set; } = "success";  // Type of the alert (success, danger, warning, etc.)
        [Parameter] public string AlertHeading { get; set; } = "Success!";  // Alert heading text
        [Parameter] public string AlertMessage { get; set; } = "Operation completed successfully.";  // Alert body message
        [Parameter] public bool Show { get; set; }  // To control the visibility of the alert
        [Parameter] public EventCallback OnClose { get; set; }  // Event callback for when the alert is closed

        protected override async Task OnParametersSetAsync()
        {
            // Trigger auto-hide whenever the alert is shown
            if (Show)
            {
                await ShowAlert();
            }
        }

        protected void CloseAlert()
        {
            Show = false;
            OnClose.InvokeAsync();  // Trigger an event when the alert is closed
            StateHasChanged();
        }

        public async Task ShowAlert(int autoDismissSeconds = 5)
        {
            StateHasChanged();  // Force the UI update

            if (autoDismissSeconds > 0)
            {
                await Task.Delay(autoDismissSeconds * 1000);  // Wait for the given seconds
                CloseAlert();
            }
        }
    }

}
