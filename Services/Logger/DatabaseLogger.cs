using Imarat_Shariah.Data;
using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Services.Logger
{
    public class DatabaseLogger : ILogger
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _category;

        public DatabaseLogger(ApplicationDbContext dbContext, string category)
        {
            _dbContext = dbContext;
            _category = category;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel)) return;

            var logEntry = new LogEntry
            {
                LogLevel = logLevel,
                Message = formatter(state, exception),
                EventId = eventId.ToString(),
                Timestamp = DateTime.UtcNow,
                Exception = exception?.Message,
                StackTrace = exception?.StackTrace,
                Category = _category
            };

            _dbContext.logEntries.Add(logEntry);
            _dbContext.SaveChanges();
        }
    }

}
