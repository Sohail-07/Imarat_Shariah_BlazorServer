using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;

namespace Imarat_Shariah.Services
{
    public class SiyajatService : ISiyajatService
    {
        private readonly IRepository<Siyajat> _siyajatGenricRepository;
        private readonly ISiyajatRepository _siyajatRepository;
        private readonly AuthenticationStateProvider _authStateProvider;
        public SiyajatService(IRepository<Siyajat> siyajatGenericRepository, ISiyajatRepository siyajatRepository, AuthenticationStateProvider authStateProvider)
        {
            _siyajatGenricRepository = siyajatGenericRepository;
            _siyajatRepository = siyajatRepository;
            _authStateProvider = authStateProvider;
        }

        public async Task<Siyajat> GetByIdAsync(int id)
        {
            return await _siyajatGenricRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Siyajat>> GetAllAsync(int pageNumber, int pageSize)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Double secure data layer constraint check
            if (!user.HasClaim("Permission", "Permissions.Siyajat.View") && !user.IsInRole("Admin"))
            {
                throw new UnauthorizedAccessException("Security Breach Alert: Create operation was rejected by server.");
            }

            return await _siyajatGenricRepository.GetAllAsync(pageNumber,pageSize);
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _siyajatGenricRepository.GetTotalCountAsync();
        }

        public async Task AddAsync(Siyajat siyajat)
        {
            // Set CreatedDate to IST
            siyajat.NikahDate = siyajat.NikahDate.ToUniversalTime();
            siyajat.CreatedDate = DateTime.Now.ToUniversalTime();
            siyajat.IsActive = true;
            await _siyajatGenricRepository.AddAsync(siyajat);
        }

        public async Task UpdateAsync(Siyajat siyajat)
        {
            // Set ModifiedDate to IST
            siyajat.ModifiedDate = DateTime.Now.ToUniversalTime();
            await _siyajatGenricRepository.UpdateAsync(siyajat);
        }

        public async Task DeleteAsync(int id)
        {
            var siyajat = await _siyajatGenricRepository.GetByIdAsync(id);
            if (siyajat == null) throw new KeyNotFoundException("Siyajat not found");

            siyajat.IsActive = false;
            siyajat.DeletedDate = DateTime.Now.ToUniversalTime();
            await _siyajatGenricRepository.UpdateAsync(siyajat);
        }

        public async Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize)
        {
            return await _siyajatRepository.SearchSiyajatAsync(searchParams,pageNumber,pageSize);
        }

        public async Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams)
        {
            return await _siyajatRepository.GetTotalCountForSearchAsync(searchParams);
        }
    }
}
