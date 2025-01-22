using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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

        [Parameter]
        public string? PreviewFileUrl { get; set; }

        [Inject]
        IFileManagement _fileManagemnt { get; set; }


        private Siyajat NewFileRecord = new();
        private string? ErrorMessage;
        bool IsLoading { get; set; }

        private IBrowserFile? SelectedFile;

        private async Task HandleValidSubmit()
        {
            IsLoading = true;
            await OnSubmit.InvokeAsync(SiyajatModel);
            await CloseModal(); // Close modal after submit
            IsLoading = false;
            StateHasChanged();
        }

        private async Task CloseModal()
        {
            IsModalVisible = false; // Set modal visibility to false
            await IsModalVisibleChanged.InvokeAsync(IsModalVisible); // Notify parent component
            await OnCancel.InvokeAsync();
        }

        private async Task RemovePDF()
        {
            IsLoading = true;
            SiyajatModel.PDFPath = null;
            SiyajatModel.PreviewFileUrl = null;
            IsLoading = false;
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
                SiyajatModel.PreviewFileUrl = PreviewFileUrl;
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
    }
}
