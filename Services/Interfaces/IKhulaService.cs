using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface IKhulaService
    {
        Task<Khula> GetByIdAsync(int id);
        Task<IEnumerable<Khula>> GetAllAsync(int pageNumber, int pageSize);
        Task AddAsync(Khula khula);
        Task UpdateAsync(Khula khula);
        Task DeleteAsync(int id);
    }
}
