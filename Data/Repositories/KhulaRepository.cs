using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Data.Repositories
{
    public class KhulaRepository : IKhulaRepository
    {
        private readonly ApplicationDbContext _context;

        public KhulaRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Khula>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize)
        {
            var query = _context.khulas.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.FormNo))
            {
                query = query.Where(s => s.FormNumber!.ToLower().Trim() == searchParams.FormNo.ToLower().Trim());
            }

            if (!string.IsNullOrEmpty(searchParams.GroomName))
            {
                query = query.Where(s => s.GroomName!.ToLower().Trim().Contains(
                                         searchParams.GroomName.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(searchParams.BrideName))
            {
                query = query.Where(s => s.BrideName!.ToLower().Trim().Contains(
                                         searchParams.BrideName.ToLower().Trim()));
            }

            // Sorting logic
            if (!string.IsNullOrEmpty(searchParams.SortBy))
            {
                query = searchParams.SortBy switch
                {
                    "FormNo" => query.OrderBy(s => s.FormNumber),
                    "NikahDate" => query.OrderBy(s => s.NikahDate),
                    _ => query
                };
            }

            return await query
                     .Skip((pageNumber - 1) * pageSize)
                     .Take(pageSize)
                     .ToListAsync();
        }

        // Method to get the total count of filtered records for search
        public async Task<int> GetTotalCountForSearchAsync(SearchParamsModel searchParams)
        {
            var query = _context.khulas.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.FormNo))
            {
                query = query.Where(s => s.FormNumber!.ToLower().Trim().Contains(
                                         searchParams.FormNo.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(searchParams.GroomName))
            {
                query = query.Where(s => s.GroomName!.ToLower().Trim().Contains(
                                        searchParams.GroomName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParams.BrideName))
            {
                query = query.Where(s => s.BrideName!.ToLower().Trim().Contains(
                                        searchParams.BrideName.ToLower().Trim()));
            }

            // Return the count of filtered records
            return await query.CountAsync();
        }
    }
}
