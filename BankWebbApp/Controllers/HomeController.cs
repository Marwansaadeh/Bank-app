using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BankWebbApp.Models;
using BankAppData.Models;
using Microsoft.EntityFrameworkCore;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace BankWebbApp.Controllers
{
    [Authorize(Roles= "Admin,Cashier")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BankAppDataContext _context;
        public HomeController(ILogger<HomeController> logger, BankAppDataContext context)
        {
            _context = context;
            _logger = logger;
        }

        [ResponseCache(Duration =30, Location = ResponseCacheLocation.Any)]
        public IActionResult Index()
        {

            var model = new StatisticsViewModel();
            model.NumberOfAccounts = _context.Accounts.Count();
            model.NumberOfCustomers = _context.Customers.Count();
            model.SumofAccountsBalances = _context.Accounts.Sum(x => x.Balance);

            return View(model);
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
