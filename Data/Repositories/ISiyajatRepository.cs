using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Data.Repositories
{
    public interface ISiyajatRepository : IRepository<Siyajat>
    {
        Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SiyajatSearchParamsModel searchParams, int pageNumber, int pageSize);
        Task<int> GetTotalCountForSearchAsync(SiyajatSearchParamsModel searchParams);
    }
}
