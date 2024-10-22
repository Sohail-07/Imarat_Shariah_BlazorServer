using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.Pages
{
    public partial class SiyajatPage
    {
        [Inject]
        ISiyajatService _siyajatService {  get; set; }

        private List<Siyajat> SiyajatEntries = new List<Siyajat>();
        private Siyajat selectedSiyajat = new Siyajat();

        private bool isEditMode = false;

        protected override async Task OnInitializedAsync()
        {
            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
        }

        private void HandleEdit(int id)
        {
            selectedSiyajat = SiyajatEntries.FirstOrDefault(s => s.Id == id);
            isEditMode = true;
        }

        private async Task HandleDelete(int id)
        {
            await _siyajatService.DeleteAsync(id);
            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
        }

        private async Task HandleSubmit(Siyajat siyajat)
        {
            if (isEditMode)
            {
                await _siyajatService.UpdateAsync(siyajat);
            }
            else
            {
                await _siyajatService.AddAsync(siyajat);
            }

            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
            isEditMode = false;
            selectedSiyajat = new Siyajat(); // Reset the form
        }

        private void HandleCancel()
        {
            isEditMode = false;
            selectedSiyajat = new Siyajat(); // Reset the form
        }
    }
}
