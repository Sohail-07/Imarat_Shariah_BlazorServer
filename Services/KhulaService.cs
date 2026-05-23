using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;
using Imarat_Shariah.Utilities;
using Microsoft.AspNetCore.Components.Authorization;

namespace Imarat_Shariah.Services
{
    public class KhulaService : IKhulaService
    {
        private readonly IRepository<Khula> _khulaGenericRepository;
        private readonly IKhulaRepository _khulaRepository;
        private readonly AuthenticationStateProvider _authStateProvider;

        // Constructor me AuthenticationStateProvider inject kiya gya h
        public KhulaService(
            IRepository<Khula> khulaGenericRepository,
            IKhulaRepository khulaRepository,
            AuthenticationStateProvider authStateProvider)
        {
            _khulaGenericRepository = khulaGenericRepository;
            _khulaRepository = khulaRepository;
            _authStateProvider = authStateProvider;
        }

        // Reusable Helper Method for Khula validation context
        private async Task ValidatePermissionAsync(string requiredPermission, string actionName)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            // Admin handles anything. Staff requires strict centralized claim match.
            var isAuthorized = user.IsInRole(ApplicationPermissions.Roles.Admin) ||
                               user.HasClaim(ApplicationPermissions.PermissionClaimType, requiredPermission);

            if (!isAuthorized)
            {
                throw new UnauthorizedAccessException($"Security Breach Alert: {actionName} operation on Khula module was rejected by the server due to insufficient privileges.");
            }
        }

        public async Task<Khula> GetByIdAsync(int id)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.View, nameof(GetByIdAsync));
            return await _khulaGenericRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Khula>> GetAllAsync(int pageNumber, int pageSize)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.View, nameof(GetAllAsync));
            return await _khulaGenericRepository.GetAllAsync(pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountAsync()
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.View, nameof(GetTotalCountAsync));
            return await _khulaGenericRepository.GetTotalCountAsync();
        }

        public async Task AddAsync(Khula khula)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.Create, nameof(AddAsync));

            // UTC standards for PostgreSQL mapping
            khula.NikahDate = khula.NikahDate.ToUniversalTime();
            khula.CreatedDate = DateTime.Now.ToUniversalTime();
            khula.IsActive = true;

            await _khulaGenericRepository.AddAsync(khula);
        }

        public async Task UpdateAsync(Khula khula)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.Update, nameof(UpdateAsync));

            khula.NikahDate = khula.NikahDate.ToUniversalTime();
            khula.ModifiedDate = DateTime.Now.ToUniversalTime();

            await _khulaGenericRepository.UpdateAsync(khula);
        }

        public async Task DeleteAsync(int id)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.Delete, nameof(DeleteAsync));

            var khula = await _khulaGenericRepository.GetByIdAsync(id);
            if (khula == null) throw new KeyNotFoundException("Khula record was not found on host context database.");

            khula.IsActive = false;
            khula.DeletedDate = DateTime.Now.ToUniversalTime();

            await _khulaGenericRepository.UpdateAsync(khula);
        }

        public async Task<IEnumerable<Khula>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.View, nameof(SearchSiyajatAsync));
            return await _khulaRepository.SearchSiyajatAsync(searchParams, pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams)
        {
            await ValidatePermissionAsync(ApplicationPermissions.Khula.View, nameof(GetTotalCountForSearchAsync));
            return await _khulaRepository.GetTotalCountForSearchAsync(searchParams);
        }
    }
}