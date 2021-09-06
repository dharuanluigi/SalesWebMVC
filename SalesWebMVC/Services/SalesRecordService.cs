using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMVC.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMVCContext _context;

        public SalesRecordService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            // IQueryble -> query constructory
            var result = from sales in _context.SalesRecord select sales;

            // filtering result from query if dates has values
            if(minDate.HasValue)
            {
                result = result.Where(s => s.Date >= minDate.Value);
            }

            if(maxDate.HasValue)
            {
                result = result.Where(s => s.Date <= maxDate.Value);
            }

            // multiples joins
            return await result
                .Include(s => s.Seller)
                .Include(s => s.Seller.Department)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }
    }
}
