using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
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

        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Inject]
        IFileManagement _fileManagemnt { get; set; }

        [Inject]
        FileManager FileManager { get; set; }

        private Siyajat NewFileRecord = new();
        private string? ErrorMessage;
        bool IsLoading { get; set; }

        private IBrowserFile? SelectedFile;

        private async Task HandleValidSubmit()
        {
            await HandleFileAdd();
            await OnSubmit.InvokeAsync(SiyajatModel);
            await CloseModal(); // Close modal after submit
        }

        private async Task CloseModal()
        {
            IsModalVisible = false; // Set modal visibility to false
            await IsModalVisibleChanged.InvokeAsync(IsModalVisible); // Notify parent component
            await OnCancel.InvokeAsync();
        }

        private async Task RemovePDF()
        {
            SiyajatModel.PDFPath = null;
            SiyajatModel.PreviewFileUrl = null;
        }

        private string GetTitle() => IsEditMode ? "Edit Siyajat Entry" : "Add New Siyajat";

        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            SelectedFile = e.File;

            if (SelectedFile != null)
            {
                var data = await _fileManagemnt.HandelSelectedFile(SelectedFile);

                PreviewFileUrl = data.Item1;
                SelectedFile = data.Item2;
                SiyajatModel.PreviewFileUrl = PreviewFileUrl;
            }

            StateHasChanged();
        }

        private async Task HandleFileAdd()
        {
            IsLoading = true; // Start loading spinner

            // Set a larger maximum file size limit (e.g., 5 MB = 5 * 1024 * 1024)
            long maxAllowedSize = 5 * 1024 * 1024;
            if (SelectedFile != null)
            {
                var fileExtension = Path.GetExtension(SelectedFile.Name).ToLower(); // Get file extension and convert to lower case

                // Check if the file is a PDF
                if (fileExtension != ".pdf")
                {
                    // Show error message if the file is not a PDF
                    ErrorMessage = "Only PDF files are allowed.";
                    IsLoading = false; // Stop loading spinner
                    return;
                }

                var newFileName = SiyajatModel.QazatNo + fileExtension; // Rename the file with user-provided name + extension

                var filePath = FileManager.GetFilePath(newFileName); // Use new file name with extension

                // Save the file to the local directory
                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await SelectedFile.OpenReadStream(maxAllowedSize).CopyToAsync(fileStream);

                // Set file path in the new record
                SiyajatModel.PDFPath = filePath;
                // Save the record in the database
                //await FileRepo.AddFileAsync(NewFileRecord);

                // Reset the form
                NewFileRecord = new Siyajat();
                SelectedFile = null;
                PreviewFileUrl = null;
            }

            IsLoading = false; // Stop loading spinner
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
