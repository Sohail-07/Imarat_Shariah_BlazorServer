using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class KhulaForm
    {
        [Parameter]
        public Khula KhulaModel { get; set; }

        [Parameter]
        public EventCallback<Khula> OnSubmit { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        [Parameter]
        public bool IsEditMode { get; set; }

        [Parameter]
        public bool IsModalVisible { get; set; }

        [Parameter]
        public EventCallback<bool> IsModalVisibleChanged { get; set; }

        [Parameter]
        public string? PreviewFileUrl { get; set; }

        [Inject]
        IFileManagement _fileManagemnt { get; set; }

        private IBrowserFile? SelectedFile;
        private string? ErrorMessage;
        bool IsLoading { get; set; }

        private async Task HandleValidSubmit()
        {
            IsLoading = true;
            await OnSubmit.InvokeAsync(KhulaModel);
            await CloseModal(); // Close modal after submit
            IsLoading = false;
        }

        private async Task CloseModal()
        {
            IsModalVisible = false; // Set modal visibility to false
            await IsModalVisibleChanged.InvokeAsync(IsModalVisible); // Notify parent component
            await OnCancel.InvokeAsync();
        }

        private string GetTitle() => IsEditMode ? "Edit Siyajat Entry" : "Add New Siyajat";

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            IsLoading = true;

            SelectedFile = e.File;

            if (SelectedFile != null)
            {
                var data = await _fileManagemnt.HandelSelectedFile(SelectedFile);

                PreviewFileUrl = data.Item1;
                SelectedFile = data.Item2;
                KhulaModel.PreviewFileUrl = PreviewFileUrl;
            }
            IsLoading = false;
            StateHasChanged();
        }

        private void HandleDrop(DragEventArgs e)
        {
            // Handle PDF drag-drop logic
        }

        private void HandleDragOver(DragEventArgs e)
        {
            // Handle drag-over logic
        }

        private async Task RemovePDF()
        {
            KhulaModel.PDFPath = null;
            KhulaModel.PreviewFileUrl = null;
        }

    }
}
