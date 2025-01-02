using Imarat_Shariah.Data;

namespace Imarat_Shariah.Services.Logger
{
    public class DatabaseLoggerProvider : ILoggerProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DatabaseLoggerProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            return new DatabaseLogger(dbContext, categoryName);
        }

        public void Dispose() { }
    }

}
