using Imarat_Shariah.Components.ViewModels;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class SiyajatSearchBarComponent
    {
        [Parameter]
        public EventCallback<SiyajatSearchParamsModel> OnSearch { get; set; }

        public int? SearchFormNo { get; set; } = null;
        public int? SearchQazatNo { get; set; } = null!;
        public string? SearchGroomName { get; set; }
        public string? SearchBrideName { get; set; }
        public string? FilterFormType { get; set; }
        public string? SortBy { get; set; }

        private async Task Search()
        {
            var searchParams = new SiyajatSearchParamsModel
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

        void Reset()
        {
            SearchFormNo = null;
            SearchQazatNo = null;
            SearchGroomName = null;
            SearchBrideName = null;
            FilterFormType = null;
            SortBy = null;
        }
    }
}
