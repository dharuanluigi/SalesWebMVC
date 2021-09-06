using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SalesWebMVC.Models
{
    public class Seller
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "{0} is mandatory")]
        [StringLength(60, MinimumLength = 3, ErrorMessage = "{0} must be size {2} between {1} caracters")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "{0} is mandatory")]
        [EmailAddress(ErrorMessage = "Enter a valid mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "{0} is mandatory")]
        [Range(100.00, 50000.00, ErrorMessage = "{0} must be between {1} until {2}")]
        [Display(Name = "Base Salary")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double BaseSalary { get; set; }

        [Required(ErrorMessage = "{0} is mandatory")]
        [Display(Name = "Birth Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime BirthDate { get; set; }

        public Department Department { get; set; }

        public int DepartmentId { get; set; }

        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();

        public Seller()
        {

        }

        public Seller(int id, string name, string email, double baseSalary, DateTime birthDate, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BaseSalary = baseSalary;
            BirthDate = birthDate;
            Department = department;
        }

        public void AddSales(SalesRecord sr)
        {
            this.Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr)
        {
            this.Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final)
        {
            return this.Sales.Where(s => s.Date >= initial && s.Date <= final).Sum(s => s.Amount);
        }
    }
}
