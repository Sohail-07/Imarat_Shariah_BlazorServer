using Imarat_Shariah.Data.Entities;
using Imarat_Shariah.Data.Repositories;
using Imarat_Shariah.Services.Interfaces;

namespace Imarat_Shariah.Services
{
    public class KhulaService : IKhulaService
    {
        private readonly IRepository<Khula> _khulaRepository;

        public KhulaService(IRepository<Khula> khulaRepository)
        {
            _khulaRepository = khulaRepository;
        }

        public async Task<Khula> GetByIdAsync(int id)
        {
            return await _khulaRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Khula>> GetAllAsync(int pageNumber, int pageSize)
        {
            return await _khulaRepository.GetAllAsync(pageNumber, pageSize);
        }

        public async Task AddAsync(Khula khula)
        {
            // Set CreatedDate to IST
            khula.NikahDate = khula.NikahDate.ToUniversalTime();
            khula.CreatedDate = DateTime.Now.ToUniversalTime();
            khula.IsActive = true;
            await _khulaRepository.AddAsync(khula);
        }

        public async Task UpdateAsync(Khula khula)
        {
            // Set ModifiedDate to IST
            khula.NikahDate = khula.NikahDate.ToUniversalTime();
            khula.ModifiedDate = DateTime.Now.ToUniversalTime();
            await _khulaRepository.UpdateAsync(khula);
        }

        public async Task DeleteAsync(int id)
        {
            var khula = await _khulaRepository.GetByIdAsync(id);
            if (khula == null) throw new KeyNotFoundException("Khula not found");

            khula.IsActive = false;
            khula.DeletedDate = DateTime.Now.ToUniversalTime();
            await _khulaRepository.UpdateAsync(khula);
        }

        private DateTime GetIndianStandardTime()
        {
            TimeZoneInfo istZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Kolkata");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, istZone);
        }
    }
}
