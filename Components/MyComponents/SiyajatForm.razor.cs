using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

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

        [Inject] IFileManagement _fileManagement { get; set; } = default!;
        [Inject] private IJSRuntime _jsRuntime { get; set; } = default!;

        private Siyajat NewFileRecord = new();
        private string? ErrorMessage;
        private string? TransientPreviewUrl; // In-memory components local state for new uploads
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
                var data = await _fileManagement.HandelSelectedFile(SelectedFile);

                PreviewFileUrl = data.Item1;
                SelectedFile = data.Item2;
                SiyajatModel.PreviewFileUrl = PreviewFileUrl;
            }
            IsLoading = false;
            StateHasChanged();
        }

        //private async Task HandleFileSelected(InputFileChangeEventArgs e)
        //{
        //    IsLoading = true;
        //    SelectedFile = e.File;

        //    if (SelectedFile != null)
        //    {
        //        // Pass the file and the entity type name (e.g., "Siyajat" or "Khula")
        //        var result = await _fileManagement.ProcessAndSaveFileAsync(SelectedFile, "Siyajat");

        //        if (result.IsSuccess)
        //        {
        //            // Fix 1: Service se naya PreviewUrl assign ho raha hai
        //            PreviewFileUrl = result.PreviewUrl;

        //            // Fix 2: Model ki sahi property (PDFPath aur PreviewFileUrl) map ho rahi hain
        //            SiyajatModel.PDFPath = result.SavedPath;
        //            SiyajatModel.PreviewFileUrl = result.PreviewUrl;
        //        }
        //        else
        //        {
        //            // UI Error Handling
        //            await _jsRuntime.InvokeVoidAsync("alert", result.ErrorMessage);

        //            SelectedFile = null;
        //            PreviewFileUrl = null;
        //            SiyajatModel.PDFPath = null;
        //            SiyajatModel.PreviewFileUrl = null;
        //        }
        //    }

        //    IsLoading = false;
        //    StateHasChanged();
        //}


        // Smart Preview Decider Logic
        //private string? GetPreviewUrl()
        //{
        //    // Case 1: Agar user ne abhi naye se koi file browser se select ki hai, to Base64 stream use karo
        //    if (!string.IsNullOrEmpty(TransientPreviewUrl))
        //    {
        //        return TransientPreviewUrl;
        //    }

        //    // Case 2: Agar user Edit Mode me hai aur DB se file ka path aa chuka hai
        //    if (!string.IsNullOrEmpty(SiyajatModel.PDFPath))
        //    {
        //        // Hamein backend pipeline par hit marna hoga relative path ke sath 
        //        // Escape URL string to safely handle forward slashes
        //        return $"/api/files/download?path={Uri.EscapeDataString(SiyajatModel.PDFPath)}";
        //    }

        //    return null;
        //}

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
