using Imarat_Shariah.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Data.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LogRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LogEntry>> GetAllLogsAsync()
        {
            var logs = await _dbContext.logEntries.ToListAsync();
            return logs;
        } 
    }
}
