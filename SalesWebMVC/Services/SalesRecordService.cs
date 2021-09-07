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
            result = minDate != null ? result.Where(s => s.Date >= minDate.Value) : result;

            result = maxDate != null ? result.Where(s => s.Date <= maxDate.Value) : result;

            return await result
                .Include(s => s.Seller)
                .Include(s => s.Seller.Department)
                .OrderByDescending(s => s.Date)
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department, SalesRecord>>> FindByGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            // IQueryble -> query constructory
            var result = from sales in _context.SalesRecord select sales;

            // filtering result from query if dates has values
            result = minDate != null ? result.Where(s => s.Date >= minDate.Value) : result;

            result = maxDate != null ? result.Where(s => s.Date <= maxDate.Value) : result;

            return await result
                .Include(s => s.Seller)
                .Include(s => s.Seller.Department)
                .OrderByDescending(s => s.Date)
                .GroupBy(s => s.Seller.Department)
                .ToListAsync();
        }
    }
}
