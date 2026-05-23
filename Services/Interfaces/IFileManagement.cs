using Microsoft.AspNetCore.Components.Forms;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface IFileManagement
    {
        Task<(string, IBrowserFile)> HandelSelectedFile(IBrowserFile file);
        Task<(bool IsSuccess, string? ErrorMessage, string? SavedPath, string? PreviewUrl)> ProcessAndSaveFileAsync(IBrowserFile file, string entityType);
    }
}
