namespace Imarat_Shariah.Components.Pages
{
    public partial class Logs
    {
        private List<LogEntry> LogEntries { get; set; } = new List<LogEntry>();
        private LogFilter Filter { get; set; } = new LogFilter();
        private int CurrentPage { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private int TotalCount { get; set; } = 0;

        private async Task LoadLogs()
        {
            //var response = await Http.GetFromJsonAsync<PaginatedLogs>($"api/logs?logLevel={Filter.LogLevel}&startDate={Filter.StartDate}&endDate={Filter.EndDate}&page={CurrentPage}&pageSize={PageSize}");
            //if (response != null)
            //{
            //    LogEntries = response.Items;
            //    TotalCount = response.TotalCount;
            //}
        }

        private async Task ApplyFilter()
        {
            CurrentPage = 1;
            await LoadLogs();
        }

        private async Task ResetFilter()
        {
            Filter = new LogFilter();
            CurrentPage = 1;
            await LoadLogs();
        }

        private async Task PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                await LoadLogs();
            }
        }

        private async Task NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                await LoadLogs();
            }
        }

        private int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        private string GetBadgeColor(string logLevel) => logLevel.ToLower() switch
        {
            "trace" => "secondary",
            "debug" => "info",
            "information" => "primary",
            "warning" => "warning",
            "error" => "danger",
            "critical" => "dark",
            _ => "light"
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadLogs();
        }

        private class LogFilter
        {
            public string LogLevel { get; set; }
            public DateTime? StartDate { get; set; }
            public DateTime? EndDate { get; set; }
        }

        private class PaginatedLogs
        {
            public List<LogEntry> Items { get; set; } = new();
            public int TotalCount { get; set; }
        }

        private class LogEntry
        {
            public int Id { get; set; }
            public string LogLevel { get; set; }
            public string Message { get; set; }
            public DateTime Timestamp { get; set; }
            public string Category { get; set; }
        }
    }
}
