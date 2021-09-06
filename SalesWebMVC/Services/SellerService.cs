using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using Microsoft.EntityFrameworkCore;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Services
{
    public class SellerService
    {
        private readonly SalesWebMVCContext _context;

        public SellerService(SalesWebMVCContext context)
        {
            _context = context;
        }

        public async Task<List<Seller>> FindAllAsync()
        {
            return await _context.Seller.OrderBy(s => s.Name).ToListAsync();
        }

        public async Task InsertAsync(Seller seller)
        {
            _context.Add(seller);
            await _context.SaveChangesAsync();
        }

        public async Task<Seller> FinByIdAsync(int id)
        {
            // include makes like join
            return await _context.Seller.Include(s => s.Department).FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task DeleteAsync(int id)
        {
            var seller = await FinByIdAsync(id);
            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Seller seller)
        {
            try
            {
                bool hasAny = await _context.Seller.AnyAsync(s => s.Id == seller.Id);
                if (!hasAny)
                {
                    throw new NotFoundException("User not found");
                }

                _context.Update(seller);
                await _context.SaveChangesAsync();
            }
            catch(DbConcurrencyException e)
            {
                throw new DbConcurrencyException(e.Message);
            }
        }
    }
}
