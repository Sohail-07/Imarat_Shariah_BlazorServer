namespace Imarat_Shariah.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> SaveFileAsync(Stream fileStream, string fileName, string subFolder);
        Task<Stream> GetFileAsync(string fileName);
        Task DeleteFileAsync(string fileName);
    }
}
