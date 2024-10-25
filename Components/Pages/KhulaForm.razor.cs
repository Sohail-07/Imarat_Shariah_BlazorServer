using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.Pages
{
    public partial class KhulaForm
    {
        [Inject]
        IKhulaService _khulaService { get; set; }

        [Inject]
        ITimeConversion timeConversion { get; set; }

        private Khula khula = new Khula();
        private List<Khula>? khulas;

        protected override async Task OnInitializedAsync()
        {
            khulas = (await _khulaService.GetAllAsync(1,100)).ToList();
        }

        private async Task HandelSubmit()
        {
            if (khula.Id > 0)
            {
                await _khulaService.UpdateAsync(khula);
            }
            else
            {
                await _khulaService.AddAsync(khula);
            }
            khulas = (await _khulaService.GetAllAsync(1, 100)).ToList();
            khula = new Khula(); // Reset form
        }

        private async Task EditKhula(Khula khula)
        {
            this.khula = khula; // Bind existing data to form for editing
        }

        private async Task DeleteKhula(int id)
        {
            await _khulaService.DeleteAsync(id);
            khulas = (await _khulaService.GetAllAsync(1, 100)).ToList();
        }
    }
}
