namespace Imarat_Shariah.Services
{
    public class FileManager
    {
        public static string BasePath => Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

        public static string EnsureDirectoryExists()
        {
            if (!Directory.Exists(BasePath))
                Directory.CreateDirectory(BasePath);

            return BasePath;
        }

        public static string GetFilePath(string fileName) => Path.Combine(EnsureDirectoryExists(), fileName);
    }
}
