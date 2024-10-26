using Imarat_Shariah.Components.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class KhulaSearchBarComponent
    {
        [Parameter]
        public EventCallback<SiyajatSearchParamsModel> OnSearch { get; set; } = new();

        public string? SearchFormNo { get; set; } = null;
        public string? SearchGroomName { get; set; }
        public string? SearchBrideName { get; set; }
        public string? SortBy { get; set; }

        private async Task Search()
        {
            var searchParams = new SiyajatSearchParamsModel
            {
                FormNo = SearchFormNo,
                GroomName = SearchGroomName,
                BrideName = SearchBrideName,
                SortBy = SortBy
            };

            await OnSearch.InvokeAsync(searchParams);
        }

        private async Task Reset()
        {
            SearchFormNo = null;
            SearchGroomName = null;
            SearchBrideName = null;
            SortBy = null;

            await OnSearch.InvokeAsync(new SiyajatSearchParamsModel());
        }
    }
}
