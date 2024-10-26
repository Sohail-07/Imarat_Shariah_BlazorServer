using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;

namespace Imarat_Shariah.Services
{
    public class KhulaService : IKhulaService
    {
        private readonly IRepository<Khula> _khulaGenericRepository;
        private readonly IKhulaRepository _khulaRepository;

        public KhulaService(IRepository<Khula> khulaGenericRepository, IKhulaRepository khulaRepository)
        {
            _khulaGenericRepository = khulaGenericRepository;
            _khulaRepository = khulaRepository;
        }

        public async Task<Khula> GetByIdAsync(int id)
        {
            return await _khulaGenericRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Khula>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _khulaGenericRepository.GetAllAsync(pageNumber, pageSize);
        }
        public async Task<int> GetTotalCountAsync()
        {
            return await _khulaGenericRepository.GetTotalCountAsync();
        }

        public async Task AddAsync(Khula khula)
        {
            // Set CreatedDate to IST
            khula.NikahDate = khula.NikahDate.ToUniversalTime();
            khula.CreatedDate = DateTime.Now.ToUniversalTime();
            khula.IsActive = true;
            await _khulaGenericRepository.AddAsync(khula);
        }

        public async Task UpdateAsync(Khula khula)
        {
            // Set ModifiedDate to IST
            khula.NikahDate = khula.NikahDate.ToUniversalTime();
            khula.ModifiedDate = DateTime.Now.ToUniversalTime();
            await _khulaGenericRepository.UpdateAsync(khula);
        }

        public async Task DeleteAsync(int id)
        {
            var khula = await _khulaGenericRepository.GetByIdAsync(id);
            if (khula == null) throw new KeyNotFoundException("Khula not found");

            khula.IsActive = false;
            khula.DeletedDate = DateTime.Now.ToUniversalTime();
            await _khulaGenericRepository.UpdateAsync(khula);
        }

        public async Task<IEnumerable<Khula>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize)
        {
            return await _khulaRepository.SearchSiyajatAsync(searchParams, pageNumber, pageSize);
        }

        public async Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams)
        {
            return await _khulaRepository.GetTotalCountForSearchAsync(searchParams);
        }
    }
}
