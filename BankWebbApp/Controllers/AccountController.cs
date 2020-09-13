using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankAppData.Models;
using BankWebbApp.AccountTransactions;
using BankWebbApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankWebbApp.Controllers
{
    [Authorize(Roles = "Admin,Cashier")]
    public class AccountController : Controller
    {
        private readonly BankAppDataContext _context;
        public AccountController( BankAppDataContext context)
        {
            _context = context;
           
        }
        [HttpGet]
        public IActionResult LoadMore(int currentnumber,int id)
        {
            AccountViewModel model = new AccountViewModel();

            var Transactions = _context.Transactions.Where(x => x.AccountId == id).OrderByDescending(x => x.Date).ThenByDescending(x => x.TransactionId).ToList();
            model.UserAccountTransctions.CustomerTransctionTotalNumber = Transactions.Count();
          
            model.Transactions = Transactions.SkipLast(model.UserAccountTransctions.CustomerTransctionTotalNumber- currentnumber).TakeLast(20).ToList();
               
            return Json (model.Transactions);
        }

        public IActionResult AccountDetails(int id)
        {
            AccountViewModel model = new AccountViewModel();
            model.UserAccountTransctions.CurrentLoadNumber = 20;
           
             var account = _context.Accounts.FirstOrDefault(x => x.AccountId == id);
            if (account != null)
            {
                var Transactions = _context.Transactions.Where(x => x.AccountId == id).OrderByDescending(x => x.Date).ThenByDescending(x=>x.TransactionId);
                model.UserAccountTransctions.CustomerTransctionTotalNumber = Transactions.Count();
                model.AccountId = account.AccountId;
                model.Balance = account.Balance;
                model.Transactions = Transactions.Take(20);
                return View(model);
            }
            else return View("ResultNotFound");
        }
        [HttpGet]
        public IActionResult Deposit(int id)
        {
            var model = new TransactionViewModel();
            model.AccountId = id;
            
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Deposit(TransactionViewModel model)
        {
            var Account = _context.Accounts.FirstOrDefault(x => x.AccountId == model.AccountId);
            if (ModelState.IsValid)
            {
                Deposit deposit = new Deposit(_context);
                deposit.Account = Account;
                deposit.Amount = model.Amount;
                deposit.Symbol = model.Symbol;
                deposit.SaveTransaction();
                model.Confirmation = "Deposit transaction has been done successfuly";

            }
            return View(model);
        }

        public IActionResult Withdraw(int id)
        {
            var model = new TransactionViewModel();
            model.AccountId = id;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Withdraw(TransactionViewModel model)
        {
            var Account = _context.Accounts.FirstOrDefault(x => x.AccountId == model.AccountId);
            if (ModelState.IsValid)
            {
                Withdraw withdraw = new Withdraw(_context);
                withdraw.Account = Account;
                withdraw.Amount = model.Amount;
                withdraw.Symbol = model.Symbol;
                withdraw.SaveTransaction();
                model.Confirmation = "Withdraw transaction has been done successfuly";

            }
            return View(model);
        }
        public IActionResult Transfer(int id)
        {
            var model = new TransferTransactionViewModel();
            model.AccountId = id;

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Transfer(TransferTransactionViewModel model)
        {
            var Account = _context.Accounts.FirstOrDefault(x => x.AccountId == model.AccountId);
            var RecieverAccount = _context.Accounts.FirstOrDefault(x => x.AccountId == model.ReceivedAccount);
            if (ModelState.IsValid)
            {
                Transfer transfer = new Transfer(_context);
                transfer.Account = Account;
                transfer.Amount = model.Amount;
                transfer.Symbol = model.Symbol;
                transfer.Bank = model.Bank;
                transfer.ReceivedAccount = RecieverAccount;
                transfer.SaveTransaction();
                model.Confirmation = "Transfer transaction has been done successfuly";

            }
            return View(model);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyAmount(decimal Amount,int AccountId)
        {
            var Account = _context.Accounts.FirstOrDefault(x => x.AccountId == AccountId);

            if (Amount> Account.Balance)
            {
                return Json("You can't transer more amount that exist in your account");
            }

            return Json(true);
        }

        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyReceivedAccount(int ReceivedAccount, int AccountId)
        {
            var receivedaccount = _context.Accounts.FirstOrDefault(x => x.AccountId == ReceivedAccount);
            if (ReceivedAccount == AccountId)
            {

            return Json("You can't transer to the same account");
            }
           else if (receivedaccount==null)
            {

                return Json("Recievedaccount can't be found, please enter a valid account number");
            }
            return Json(true);
        }

    }
}