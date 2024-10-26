using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Services.Interfaces
{
    public interface ISiyajatService
    {
        Task<Siyajat> GetByIdAsync(int id);
        Task<IEnumerable<Siyajat>> GetAllAsync(int pageNumber, int pageSize);
        Task<int> GetTotalCountAsync();
        Task AddAsync(Siyajat siyajat);
        Task UpdateAsync(Siyajat siyajat);
        Task DeleteAsync(int id);
        Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize);
        Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams);
    }
}
