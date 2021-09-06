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
        public async Task<IActionResult> Index()
        {
            var list = await _sellerService.FindAllAsync();
            return View(list);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel
            {
                Departments = departments
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Seller seller)
        {
            if(!ModelState.IsValid)
            {
                return View(InvalidStateAsync(seller));
            }

            await  _sellerService.InsertAsync(seller);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            var seller = await GetUserByIdAsync(id);

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
        public async Task<IActionResult> Delete(int id)
        {
            await _sellerService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            var seller = await GetUserByIdAsync(id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new
                {
                    message = "Seller not found"
                });
            }

            return View(seller);
        }

        private async Task<Seller> GetUserByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }

            var seller = await _sellerService.FinByIdAsync(id.Value);

            if (seller == null)
            {
                return null;
            }

            return seller;
        }

        private async Task<SellerFormViewModel> InvalidStateAsync(Seller seller)
        {
            var departments = await _departmentService.FindAllAsync();
            var viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departments
            };

            return viewModel;
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            var seller = await GetUserByIdAsync (id);

            if(seller == null)
            {
                return RedirectToAction(nameof(Error), new 
                {
                    message = "Seller not found"
                });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel
            {
                Seller = seller,
                Departments = departments
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller)
        {
            if(!ModelState.IsValid)
            {
                return View(await InvalidStateAsync(seller));
            }

            if (id != seller.Id)
            {
                return RedirectToAction(nameof(Error), new
                {
                    message = "Unrecognized Id"
                });
            }

            await _sellerService.UpdateAsync(seller);
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
