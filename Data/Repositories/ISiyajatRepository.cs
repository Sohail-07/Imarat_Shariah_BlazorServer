using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Data.Repositories
{
    public interface ISiyajatRepository
    {
        Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize);
        Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams);
    }
}
