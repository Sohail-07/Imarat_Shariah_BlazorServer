using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components;

namespace Imarat_Shariah.Components.Pages
{
    public partial class KhulaPage
    {
        [Inject]
        IKhulaService _khulaService { get; set; }

        [Inject]
        ITimeConversion timeConversion { get; set; }

        private Khula selectedKhula = new ();
        private List<Khula>? KhulaEntries { get; set; } = new();
        private List<Khula> filteredKhulaEntries = new();
        SearchParamsModel SearchParams { get; set; } = new();

        public DialogBoxModel DialogBoxModel { get; set; } = new();

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
            filteredKhulaEntries = KhulaEntries;
        }

        private async Task GetAll(int pageNumber, int pageSize)
        {
            IsLoading = true;
            //await Task.Delay(5000);

            var data = await _khulaService.GetAllAsync(pageNumber, pageSize);
            KhulaEntries = data.ToList();

            totalEntries = await _khulaService.GetTotalCountAsync();

            // Assuming total count is available, you'll have to calculate the totalPages based on that count
            totalPages = (int)Math.Ceiling((double)totalEntries / pageSize);

            IsLoading = false;
            StateHasChanged();
        }

        private void OpenAddModal()
        {
            selectedKhula = new();  // Clear the model
            isEditMode = false;
            isModalVisible = true;  // Open modal
        }

        private void HandleEdit(int id)
        {
            selectedKhula = KhulaEntries.FirstOrDefault(s => s.Id == id);
            isEditMode = true;
            isModalVisible = true;
        }
        private async Task HandleSearch(SearchParamsModel searchParams)
        {
            IsLoading = true;
            //await Task.Delay(5000);

            SearchParams = searchParams;
            KhulaEntries = (await _khulaService.SearchSiyajatAsync(SearchParams, currentPage, pageSize)).ToList();

            totalEntries = await _khulaService.GetTotalCountForSearchAsync(SearchParams);

            // Assuming total count is available, you'll have to calculate the totalPages based on that count
            totalPages = (int)Math.Ceiling((double)totalEntries / pageSize);

            IsLoading = false;
            StateHasChanged();
        }

        private void OpenDeleteConfirmation(Khula model)
        {
            // Set up the dialog box model for confirmation
            DialogBoxModel.IsVisible = true;
            DialogBoxModel.Title = "Confirm Khula Deletion";
            DialogBoxModel.Message = new MarkupString($"Are you sure you want to delete Khula<br /> <strong>Form Number {model.FormNumber} record?</strong>");
            DialogBoxModel.OnConfirm = EventCallback.Factory.Create<bool>(this, (confirm) => ConfirmDelete(model.Id));
            DialogBoxModel.OnCancel = EventCallback.Factory.Create<bool>(this, (cancel) => DialogBoxModel.IsVisible = false);
        }

        private async Task ConfirmDelete(int id)
        {
            await _khulaService.DeleteAsync(id);

            await HandleSearch(SearchParams);
            DialogBoxModel.IsVisible = false; // Close dialog
        }

        private async Task HandleSubmit(Khula khula)
        {
            if (isEditMode)
            {
                await _khulaService.UpdateAsync(khula);

                ToasterBody = $"Khula form with form number {khula.FormNumber} updated.";
            }
            else
            {
                await _khulaService.AddAsync(khula);
                ToasterBody = $"New khula form with form number {khula.FormNumber} added.";
            }

            await HandleSearch(SearchParams);
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

        private async Task DeleteKhula(int id)
        {
            await _khulaService.DeleteAsync(id);
            KhulaEntries = (await _khulaService.GetAllAsync(1, 100)).ToList();
        }

        private async Task GoToPage(int pageNumber)
        {
            if (pageNumber < 1 || pageNumber > totalPages) return;
            currentPage = pageNumber;
            await HandleSearch(SearchParams);
        }
        private async Task OnPageSizeChanged(int newSize)
        {
            pageSize = newSize;
            currentPage = 1; // Reset to first page with new page size
            await HandleSearch(SearchParams);
        }
    }
}
