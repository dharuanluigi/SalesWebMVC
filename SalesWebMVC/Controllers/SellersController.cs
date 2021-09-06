using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMVC.Services;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModel;
using SalesWebMVC.Models.ViewModels;
using System.Diagnostics;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService;
            _departmentService = departmentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel
            {
                Departments = departments
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller)
        {
            if(!ModelState.IsValid)
            {
                return View(InvalidState(seller));
            }

            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            var seller = GetUserById(id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new 
                {
                    message = "Seller not found"
                });
            }

            return View(seller);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            var seller = GetUserById(id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new
                {
                    message = "Seller not found"
                });
            }

            return View(seller);
        }

        private Seller GetUserById(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var seller = _sellerService.FinById(id.Value);

            if (seller == null)
            {
                return null;
            }

            return seller;
        }

        private SellerFormViewModel InvalidState(Seller seller)
        {
            var departments = _departmentService.FindAll();
            var viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departments
            };

            return viewModel;
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var seller = GetUserById(id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new 
                {
                    message = "Seller not found"
                });
            }

            List<Department> departments = _departmentService.FindAll();
            SellerFormViewModel viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departments
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if(!ModelState.IsValid)
            {
                return View(InvalidState(seller));
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new
                {
                    message = "Unrecognized Id"
                });
            }

            _sellerService.Update(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel
            {
                Message = message,
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            };

            return View(viewModel);
        }
    }
}
