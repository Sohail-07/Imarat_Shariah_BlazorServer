using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface ISiyajatService
    {
        Task<Siyajat> GetByIdAsync(int id);
        Task<IEnumerable<Siyajat>> GetAllAsync();
        Task AddAsync(Siyajat siyajat);
        Task UpdateAsync(Siyajat siyajat);
        Task DeleteAsync(int id);
    }
}
