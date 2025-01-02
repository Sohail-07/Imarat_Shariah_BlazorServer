namespace Imarat_Shariah.Data.Entities
{
    public class LogEntry
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public string? Message { get; set; }
        public string? EventId { get; set; }
        public DateTime Timestamp { get; set; }
        public string? Exception { get; set; }
        public string? StackTrace { get; set; }
        public string? Category { get; set; }
    }
}
