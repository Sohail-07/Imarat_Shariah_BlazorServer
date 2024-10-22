using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.ViewModels
{
    public class DialogBoxModel
    {
        public bool IsVisible { get; set; }
        public string Title { get; set; } = string.Empty;
        public MarkupString Message { get; set; } = new MarkupString("Are you sure you want to delete this item?");
        public EventCallback<bool> OnConfirm { get; set; }
        public EventCallback<bool> OnCancel { get; set; }
    }
}
