using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.MyComponents
{
    public partial class SiyajatList
    {
        [Inject]
        ITimeConversion _timeConversion { get; set; }

        [Parameter]
        public List<Siyajat> SiyajatEntries { get; set; }

        [Parameter]
        public EventCallback<int> OnEdit { get; set; }

        [Parameter]
        public EventCallback<Siyajat> OnDelete { get; set; }
    }
}
