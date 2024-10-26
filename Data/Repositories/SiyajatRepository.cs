using Imarat_Shariah.Components.ViewModels;
using Imarat_Shariah.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Imarat_Shariah.Data.Repositories
{
    public class SiyajatRepository :  ISiyajatRepository
    {
        private readonly ApplicationDbContext _context;

        public SiyajatRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        public async Task<IEnumerable<Siyajat>> SearchSiyajatAsync(SearchParamsModel searchParams, int pageNumber, int pageSize)
        {
            var query = _context.siyajats.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.FormNo))
            {
                query = query.Where(s => s.FormNo.ToLower().Trim() == searchParams.FormNo.ToLower().Trim());
            }

            if (!string.IsNullOrEmpty(searchParams.QazatNo))
            {
                query = query.Where(s => s.QazatNo.ToLower().Trim() == searchParams.QazatNo.ToLower().Trim());
            }

            if (!string.IsNullOrEmpty(searchParams.GroomName))
            {
                query = query.Where(s => s.GroomName.ToLower().Trim().Contains(
                                         searchParams.GroomName.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(searchParams.BrideName))
            {
                query = query.Where(s => s.BrideName.ToLower().Trim().Contains(
                                         searchParams.BrideName.ToLower().Trim()));
            }

            if (!string.IsNullOrEmpty(searchParams.FormType))
            {
                query = query.Where(s => s.FormType == searchParams.FormType);
            }

            // Sorting logic
            if (!string.IsNullOrEmpty(searchParams.SortBy))
            {
                query = searchParams.SortBy switch
                {
                    "FormNo" => query.OrderBy(s => s.FormNo),
                    "QazatNo" => query.OrderBy(s => s.QazatNo),
                    "NikahDate" => query.OrderBy(s => s.NikahDate),
                    "FormType" => query.OrderBy(s => s.FormType),
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
            var query = _context.siyajats.AsQueryable();

            if (!string.IsNullOrEmpty(searchParams.FormNo))
            {
                query = query.Where(s => s.FormNo.ToLower().Trim() == searchParams.FormNo.ToLower().Trim());
            }

            if (!string.IsNullOrEmpty(searchParams.QazatNo))
            {
                query = query.Where(s => s.QazatNo.ToLower().Trim() == searchParams.QazatNo.ToLower().Trim());
            }
            if (!string.IsNullOrEmpty(searchParams.GroomName))
            {
                query = query.Where(s => s.GroomName.ToLower().Trim().Contains(
                                        searchParams.GroomName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParams.BrideName))
            {
                query = query.Where(s => s.BrideName.ToLower().Trim().Contains(
                                        searchParams.BrideName.ToLower().Trim()));
            }
            if (!string.IsNullOrEmpty(searchParams.FormType))
            {
                query = query.Where(s => s.FormType == searchParams.FormType);
            }

            // Return the count of filtered records
            return await query.CountAsync();
        }
    }
}
