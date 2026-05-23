using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;
using Imarat_Shariah.Utilities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Imarat_Shariah.Services
{
    public class SiyajatService : ISiyajatService
    {
        private readonly IRepository<Siyajat> _siyajatGenricRepository;
        private readonly ISiyajatRepository _siyajatRepository;
        private readonly AuthenticationStateProvider _authStateProvider;

        public SiyajatService(
            IRepository<Siyajat> siyajatGenericRepository,
            ISiyajatRepository siyajatRepository,
            AuthenticationStateProvider authStateProvider)
        {
            _siyajatGenricRepository = siyajatGenericRepository;
            _siyajatRepository = siyajatRepository;
            _authStateProvider = authStateProvider;
        }

        // Helper Method: Reusable helper taaki har method me duplicate verification code na likhna pade
        private async Task ValidatePermissionAsync(string requiredPermission, string actionName)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Admin bypasses everything, otherwise explicitly looks for the type-safe claim
            var isAuthorized = user.IsInRole(ApplicationPermissions.Roles.Admin) ||
                               user.HasClaim(ApplicationPermissions.PermissionClaimType, requiredPermission);

            if (!isAuthorized)
            {
                throw new UnauthorizedAccessException($"Security Breach Alert: {actionName} operation on siyajat module was rejected by the server due to insufficient privileges.");
            }
        }

        public async Task<Siyajat> GetByIdAsync(int id)
        {
            // Security check before fetching details
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.View, nameof(GetByIdAsync));
            return await _siyajatGenricRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Siyajat>> GetAllAsync(int pageNumber, int pageSize)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.View, nameof(GetAllAsync));
            return await _siyajatGenricRepository.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountAsync()
        {
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.View, nameof(GetTotalCountAsync));
            return await _siyajatGenricRepository.GetTotalCountAsync();
        }

        public async Task AddAsync(Siyajat siyajat)
        {
            // Enforce compilation-safe Create Permission check
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.Create, nameof(AddAsync));

            // Set CreatedDate to UTC (EF Core + PostgreSQL best practice standard)
            siyajat.NikahDate = siyajat.NikahDate.ToUniversalTime();
            siyajat.CreatedDate = DateTime.Now.ToUniversalTime();
            siyajat.IsActive = true;

            await _siyajatGenricRepository.AddAsync(siyajat);
        }

        public async Task UpdateAsync(Siyajat siyajat)
        {
            // Enforce compilation-safe Update Permission check
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.Update, nameof(UpdateAsync));

            siyajat.ModifiedDate = DateTime.Now.ToUniversalTime();
            await _siyajatGenricRepository.UpdateAsync(siyajat);
        }

        public async Task DeleteAsync(int id)
        {
            // Enforce compilation-safe Delete Permission check
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.Delete, nameof(DeleteAsync));

            var siyajat = await _siyajatGenricRepository.GetByIdAsync(id);
            if (siyajat == null) throw new KeyNotFoundException("Siyajat record was not found on host context database.");

            siyajat.IsActive = false;
            siyajat.DeletedDate = DateTime.Now.ToUniversalTime();
            await _siyajatGenricRepository.UpdateAsync(siyajat);
        }

        public async Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.View, nameof(SearchSiyajatAsync));
            return await _siyajatRepository.SearchSiyajatAsync(searchParams, pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Siyajat.View, nameof(GetTotalCountForSearchAsync));
            return await _siyajatRepository.GetTotalCountForSearchAsync(searchParams);
        }
    }
}