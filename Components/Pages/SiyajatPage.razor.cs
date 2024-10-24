using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.Pages
{
    public partial class SiyajatPage
    {
        [Inject]
        ISiyajatService _siyajatService { get; set; }

        private List<Siyajat> filteredSiyajatEntries = new();

        public DialogBoxModel DialogBoxModel { get; set; } = new();

        SiyajatSearchParamsModel SearchParams { get; set; }

        private List<Siyajat> SiyajatEntries = new();
        private Siyajat selectedSiyajat = new();
        private bool isEditMode = false;
        private bool isModalVisible = false;

        string ToasterHeading = "Success!";
        string ToasterBody = "Added!";
        bool showToaster = false;

        protected override async Task OnInitializedAsync()
        {
            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
            filteredSiyajatEntries = SiyajatEntries;
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
        private async Task HandleSearch(SiyajatSearchParamsModel searchParams)
        {
            SearchParams = searchParams;
            // Filter and Sort the Siyajat entries based on searchParams
            SiyajatEntries = (await _siyajatService.SearchSiyajatAsync(SearchParams)).ToList();
        }
        
        private void OpenDeleteConfirmation(Siyajat model)
        {
            // Set up the dialog box model for confirmation
            DialogBoxModel.IsVisible = true;
            DialogBoxModel.Title = "Confirm Siyajat Deletion";
            DialogBoxModel.Message = new MarkupString($"Are you sure you want to delete Siyajat<br /> <strong>Form Number {model.FormNo}, Qazat Number {model.QazatNo} record?</strong>");
            DialogBoxModel.OnConfirm = EventCallback.Factory.Create<bool>(this, (confirm) => ConfirmDelete(model.Id));
            DialogBoxModel.OnCancel = EventCallback.Factory.Create<bool>(this, (cancel) => DialogBoxModel.IsVisible = false);
        }

        private async Task ConfirmDelete(int id)
        {
            await _siyajatService.DeleteAsync(id);
            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
            DialogBoxModel.IsVisible = false; // Close dialog
        }

        private async Task HandleSubmit(Siyajat siyajat)
        {
            if (isEditMode)
            {
                await _siyajatService.UpdateAsync(siyajat);

                ToasterBody = $"Siyajat form with form number {siyajat.FormNo} updated.";
            }
            else
            {
                await _siyajatService.AddAsync(siyajat);
                ToasterBody = $"New siyajat form with form number {siyajat.FormNo} added.";
            }

            SiyajatEntries = (await _siyajatService.GetAllAsync()).ToList();
            isModalVisible = false;  // Close modal

            // Show success toaster
            ShowToaster();
        }

        private void HandleCancel()
        {
            isModalVisible = false;
        }

        private async void ShowToaster()
        {
            showToaster = true;
            StateHasChanged();  // Update UI

            // Hide the alert after 5 seconds
            await Task.Delay(5000);
            HideToaster();
        }

        private void HideToaster()
        {
            showToaster = false;
            StateHasChanged();  // Force UI update
        }

    }
}
