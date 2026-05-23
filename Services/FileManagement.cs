using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Imarat_Shariah.Services
{
    public class FileManagement : IFileManagement
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly IStorageService _storageService;

        public FileManagement(IJSRuntime jSRuntime, IStorageService storageService)
        {
            _jsRuntime = jSRuntime;
            _storageService = storageService;
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
        // will use later for saving pdf files direct in new folder instead of making base64 and save in db.
        public async Task<(bool IsSuccess, string? ErrorMessage, string? SavedPath, string? PreviewUrl)> ProcessAndSaveFileAsync(IBrowserFile file, string entityType)
        {
            const long maxAllowedSize = 2 * 1024 * 1024; // 2 MB

            // 1. Validation
            var fileExtension = Path.GetExtension(file.Name).ToLowerInvariant();
            if (fileExtension != ".pdf") return (false, "Only PDF files are allowed.", null, null);
            if (file.Size > maxAllowedSize) return (false, "File size exceeds 2MB limit", null, null);

            try
            {
                // 2. Generate Base64 Preview for UI instantly
                using var tempStream = new MemoryStream();
                await file.OpenReadStream(maxAllowedSize).CopyToAsync(tempStream);
                var base64String = Convert.ToBase64String(tempStream.ToArray());
                var previewUrl = $"data:application/pdf;base64,{base64String}";

                // 3. Reset stream position and save physically via storage service
                tempStream.Position = 0;
                string savedPath = await _storageService.SaveFileAsync(tempStream, file.Name, entityType);

                return (true, null, savedPath, previewUrl);
            }
            catch (Exception ex)
            {
                return (false, ex.Message, null, null);
            }
        }
    }
}
