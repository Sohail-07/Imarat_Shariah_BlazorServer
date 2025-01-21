using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Imarat_Shariah.Services
{
    public class FileManagement : IFileManagement
    {
        private readonly IJSRuntime _jsRuntime;

        public FileManagement(IJSRuntime jSRuntime)
        {
            _jsRuntime = jSRuntime;
        }

        public async Task<(string,IBrowserFile)> HandelSelectedFile(IBrowserFile file)
        {
            // Validate file extension
            var fileExtension = Path.GetExtension(file.Name).ToLower(); // Get file extension and convert to lower case
            string previewFileUrl;
            // Check if the file is a PDF
            if (fileExtension != ".pdf")
            {
                // Show error message if the file is not a PDF
                await _jsRuntime.InvokeVoidAsync("alert", "Only PDF files are allowed.");
                file = null;
                previewFileUrl = null;
                return (previewFileUrl,file);
            }

            long maxAllowedSize = 1 * 1024 * 1024; // 5 MB max size

            if (file.Size > maxAllowedSize)
            {
                await _jsRuntime.InvokeVoidAsync("alert", "The selected file exceeds the maximum allowed size of 5 MB.");
                file = null;
                previewFileUrl = null;
                return (previewFileUrl, file);
            }

            // Generate the file preview (only for PDFs)
            var tempStream = new MemoryStream();
            await file.OpenReadStream(maxAllowedSize).CopyToAsync(tempStream);
            previewFileUrl = $"data:application/pdf;base64,{Convert.ToBase64String(tempStream.ToArray())}";
            //SiyajatModel.PreviewFileUrl = previewFileUrl;
            return (previewFileUrl, file);

        }
    }
}
