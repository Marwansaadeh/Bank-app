using BankAppData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebbApp.AccountTransactions
{
    public abstract class Transaction
    {
        public int TransactionId { get; set; }
        public Accounts Account { get; set; }
        public  decimal Amount { get; set; }
      
        public string Symbol { get; set; }

    }
}
