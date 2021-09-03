using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Data;
using SalesWebMVC.Models;
using SalesWebMVC.Models.Enums;

namespace SalesWebMVC.Data
{
    public class SeedingService
    {
        private SalesWebMVCContext _context;

        public SeedingService(SalesWebMVCContext context)
        {
            this._context = context;
        }

        public void Seed()
        {
            if(_context.Department.Any() || _context.Seller.Any() || _context.SalesRecord.Any())
            {
                return; // database has content data
            }

            Department dp = new Department
            { 
                Id = 1,
                Name = "Computers"
            };

            Seller s1 = new Seller
            {
                Id = 1,
                Name = "Bob Brown",
                Email = "bob@gmail.com",
                BirthDate = new DateTime(1998, 5, 20),
                BaseSalary = 1000.0,
                Department = dp
            };

            SalesRecord sr = new SalesRecord
            {
                Id = 1,
                Date = new DateTime(2020, 08, 12),
                Amount = 11000.0,
                Status = SaleStatus.Billed,
                Seller = s1
            };

            this._context.Department.Add(dp);
            this._context.Seller.Add(s1);
            this._context.SalesRecord.Add(sr);

            this._context.SaveChanges();
        }
    }
}
