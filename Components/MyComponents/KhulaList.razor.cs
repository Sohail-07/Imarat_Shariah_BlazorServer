using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class KhulaList
    {
        [Inject]
        ITimeConversion _timeConversion { get; set; }

        [Parameter]
        public List<Khula> KhulaEntries { get; set; }

        [Parameter]
        public EventCallback<int> OnEdit { get; set; }

        [Parameter]
        public EventCallback<Khula> OnDelete { get; set; }

        [Parameter]
        public bool IsLoading { get; set; } = true;
    }
}
