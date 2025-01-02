using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Data.Repositories
{
    public interface ILogRepository
    {
        Task<List<LogEntry>> GetAllLogsAsync();
    }
}
