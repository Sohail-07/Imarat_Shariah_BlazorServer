using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;

namespace Imarat_Shariah.Services
{
    public class SiyajatService : ISiyajatService
    {
        private readonly IRepository<Siyajat> _siyajatGenricRepository;
        private readonly ISiyajatRepository _siyajatRepository;

        public SiyajatService(IRepository<Siyajat> siyajatGenericRepository, ISiyajatRepository siyajatRepository)
        {
            _siyajatGenricRepository = siyajatGenericRepository;
            _siyajatRepository = siyajatRepository;
        }

        public async Task<Siyajat> GetByIdAsync(int id)
        {
            return await _siyajatGenricRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Siyajat>> GetAllAsync()
        {
            return await _siyajatGenricRepository.GetAllAsync();
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

        public async Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SiyajatSearchParamsModel searchParams)
        {
            return await _siyajatRepository.SearchSiyajatAsync(searchParams);
        }
    }
}
