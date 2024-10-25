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

        private bool showSuccessAlert = false;
        private bool showErrorAlert = false;

        private bool IsLoading = false;  // Initially true to show the loader

        private int currentPage = 1;
        private int pageSize = 5;
        private int totalPages = 0;
        private int totalEntries = 0;

        protected override async Task OnInitializedAsync()
        {
            await GetAll(currentPage, pageSize);
            filteredSiyajatEntries = SiyajatEntries;
        }

        private async Task GetAll(int pageNumber, int pageSize)
        {
            IsLoading = true;
            //await Task.Delay(5000);

            var siyajats = await _siyajatService.GetAllAsync(pageNumber, pageSize);
            SiyajatEntries = siyajats.ToList();

            totalEntries = await _siyajatService.GetTotalCountAsync();

            // Assuming total count is available, you'll have to calculate the totalPages based on that count
            totalPages = (int)Math.Ceiling((double)totalEntries / pageSize);

            IsLoading = false;
            StateHasChanged();
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
            IsLoading = true;
            //await Task.Delay(5000);

            SearchParams = searchParams;
            SiyajatEntries = (await _siyajatService.SearchSiyajatAsync(SearchParams, currentPage, pageSize)).ToList();

            totalEntries = await _siyajatService.GetTotalCountForSearchAsync(SearchParams);

            // Assuming total count is available, you'll have to calculate the totalPages based on that count
            totalPages = (int)Math.Ceiling((double)totalEntries / pageSize);

            IsLoading = false;
            StateHasChanged();
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
            
            await GetAll(currentPage, pageSize);
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

            await GetAll(currentPage, pageSize);
            isModalVisible = false;  // Close modal

            TriggerSuccessAlert();
            StateHasChanged();
        }

        private void HandleCancel()
        {
            isModalVisible = false;
        }

        private async void TriggerSuccessAlert()
        {
            showSuccessAlert = true;

            // You could wait for a moment before triggering auto-hide
            await Task.Delay(1);  // You can remove this delay or adjust if needed

            // Manually trigger auto-dismiss by calling the ShowAlert method
            StateHasChanged();
        }

        private void OnToasterClosed()
        {
            showSuccessAlert = false;
        }

        private async Task GoToPage(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > totalPages) return;
            currentPage = pageNumber;
            await GetAll(currentPage, pageSize);
        }
    }
}
