using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;

namespace Imarat_Shariah.Data.Repositories
{
    public interface IKhulaRepository
    {
        Task<IEnumerable<Khula>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize);
        Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams);
    }
}
