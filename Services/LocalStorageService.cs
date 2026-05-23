using Imarat_Shariah.Services.Interfaces;

namespace Imarat_Shariah.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _baseStorageFolder = @"G:\ImaratShariahFiles";

        public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subFolder)
        {
            // Path structure: C:\ImaratShariahFiles\Siyajat\2026\May\
            var currentYear = DateTime.Now.ToString("yyyy");
            var currentMonth = DateTime.Now.ToString("MMM"); // Jan, Feb, Mar...

            var targetFolder = Path.Combine(_baseStorageFolder, subFolder, currentYear, currentMonth);

            if (!Directory.Exists(targetFolder))
                Directory.CreateDirectory(targetFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var fullPath = Path.Combine(targetFolder, uniqueFileName);

            using var filePathStream = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            await fileStream.CopyToAsync(filePathStream);

            // Database me save karne ke liye relative path/name combine karke return karo
            // Isse future me "1-Click Cloud Move" bohot aasan ho jayega
            return Path.Combine(subFolder, currentYear, currentMonth, uniqueFileName);
        }

        public Task<Stream> GetFileAsync(string relativePath)
        {
            var fullPath = Path.Combine(_baseStorageFolder, relativePath);
            if (!File.Exists(fullPath)) throw new FileNotFoundException("File nahi mili bhaya!");
            return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
        }

        public Task DeleteFileAsync(string relativePath)
        {
            var fullPath = Path.Combine(_baseStorageFolder, relativePath);
            if (File.Exists(fullPath)) File.Delete(fullPath);
            return Task.CompletedTask;
        }
    }
}
