using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.Pages
{
    public partial class SiyajatPage
    {
        [Inject]
        ISiyajatService _siyajatService {  get; set; }

        private List<Siyajat> SiyajatEntries = new();
        private Siyajat selectedSiyajat = new();
        private bool isEditMode = false;
        private bool isModalVisible = false;


        protected override async Task OnInitializedAsync()
        {
            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
        }

        private void OpenAddModal()
        {
            selectedSiyajat = new Siyajat();  // Clear the model
            isEditMode = false;
            isModalVisible = true;  // Open modal
        }

        private void HandleEdit(int id)
        {
            selectedSiyajat = SiyajatEntries.FirstOrDefault(s => s.Id == id);
            isEditMode = true;
            isModalVisible = true;
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
            isModalVisible = false;  // Close modal
        }

        private void HandleCancel()
        {
            isModalVisible = false;
        }
    }
}
