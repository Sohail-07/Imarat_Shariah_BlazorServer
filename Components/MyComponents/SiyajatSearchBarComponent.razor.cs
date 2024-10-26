using Imarat_Shariah.Components.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class SiyajatSearchBarComponent
    {
        [Parameter]
        public EventCallback<SearchParamsModel> OnSearch { get; set; } = new();

        public string? SearchFormNo { get; set; } = null;
        public string? SearchQazatNo { get; set; } = null!;
        public string? SearchGroomName { get; set; }
        public string? SearchBrideName { get; set; }
        public string? FilterFormType { get; set; }
        public string? SortBy { get; set; }

        private async Task Search()
        {
            var searchParams = new SearchParamsModel
            {
                FormNo = SearchFormNo,
                QazatNo = SearchQazatNo,
                GroomName = SearchGroomName,
                BrideName = SearchBrideName,
                FormType = FilterFormType,
                SortBy = SortBy
            };

            await OnSearch.InvokeAsync(searchParams);
        }

        private async Task Reset()
        {
            SearchFormNo = null;
            SearchQazatNo = null;
            SearchGroomName = null;
            SearchBrideName = null;
            FilterFormType = null;
            SortBy = null;
            
            await OnSearch.InvokeAsync(new SearchParamsModel());
        }
    }
}
