using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAppData.Models;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankWebbApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly BankAppDataContext _context;
        public CustomerController(ILogger<CustomerController> logger, BankAppDataContext context)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult CustomerDetails(string CustomerID)
        {
            var Input = new int();

            //ViewData["GetValue"] = CustomerID;
            var model = new CustomerDetailsViewModel();
            if (int.TryParse(CustomerID, out Input))
            {
                Input = Convert.ToInt32(CustomerID);
            }

            var customer = _context.Customers.FirstOrDefault(x => x.CustomerId == Input);
            
            if (customer != null)
            {
               var customersaccounts = _context.Customers.Include(x => x.Dispositions).Where(x => x.CustomerId == customer.CustomerId).SelectMany(x => x.Dispositions).Include(x => x.Account)
                  .Select(x => x.Account);
                var total = customersaccounts.Sum(x => x.Balance);

                model.CustomerId = customer.CustomerId;
                model.Birthday = customer.Birthday;
                model.City = customer.City;
                model.Country = customer.Country;
                model.CountryCode = customer.CountryCode;
                model.Emailaddress = customer.Emailaddress;
                model.Gender = customer.Gender;
                model.Givenname = customer.Givenname;
                model.Surname = customer.Surname;
                model.Telephonecountrycode = customer.Telephonecountrycode;
                model.Telephonenumber = customer.Telephonenumber;
                model.Zipcode = customer.Zipcode;
                model.SumCustomerAccountBalnaces = total;
                model.CustomersAccounts = customersaccounts;
            }
            else
            {

                return View("ResultNotFound", CustomerID);


            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Customers(string search,string page)
        {                
            var model = new CustomerViewModel();
          
            ViewData["SearchValue"] = search;

            var query = _context.Customers.Select(c => new Items()
            {
                CustomerId = c.CustomerId,
                NationalId = c.NationalId,              
                Givenname = c.Givenname,
                City = c.City,
                Streetaddress = c.Streetaddress
            });

            int currentPage = string.IsNullOrEmpty(page) ? 1 : Convert.ToInt32(page);

            if (string.IsNullOrEmpty(search))
            {
                var pageCount = (double)query.Count() / PagingViewModel.PageSize;
                model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

                query = query.Skip((currentPage - 1) * PagingViewModel.PageSize).Take(PagingViewModel.PageSize);


                model.PagingViewModel.CurrentPage = currentPage;
                model.Items = query.ToList();
                return View(model);
            }
            else
            {
               var query2 = query.Where(s => s.City.StartsWith(search) || s.City== search || s.Givenname.StartsWith(search)|| s.Givenname==search);
                var pageCount = (double)query2.Count() / PagingViewModel.PageSize;
                model.PagingViewModel.MaxPages = (int)Math.Ceiling(pageCount);

                query2 = query2.Skip((currentPage - 1) * PagingViewModel.PageSize).Take(PagingViewModel.PageSize);


                model.PagingViewModel.CurrentPage = currentPage;
                model.Items = query2.ToList();
                return View(model);
            }
        }
       

    }
}