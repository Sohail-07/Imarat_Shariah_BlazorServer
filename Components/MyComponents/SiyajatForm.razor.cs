using Imarat_Shariah.Data.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class SiyajatForm
    {
        [Parameter]
        public Siyajat SiyajatModel { get; set; }

        [Parameter]
        public EventCallback<Siyajat> OnSubmit { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public bool IsEditMode { get; set; }

        [Parameter]
        public bool IsModalVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsModalVisibleChanged { get; set; }

        private string PdfFileName { get; set; }

        private async Task HandleValidSubmit()
        {
            await OnSubmit.InvokeAsync(SiyajatModel);
            await CloseModal(); // Close modal after submit
        }

        private async Task CloseModal()
        {
            IsModalVisible = false; // Set modal visibility to false
            await IsModalVisibleChanged.InvokeAsync(IsModalVisible); // Notify parent component
            await OnCancel.InvokeAsync();
        }

        private string GetTitle() => IsEditMode ? "Edit Siyajat Entry" : "Add New Siyajat";

        private void HandleFileSelected(ChangeEventArgs e)
        {
            // Handle file selection logic (e.g. file upload)
        }

        private void HandleDrop(DragEventArgs e)
        {
            // Handle PDF drag-drop logic
        }

        private void HandleDragOver(DragEventArgs e)
        {
            // Handle drag-over logic
        }
    }

}
