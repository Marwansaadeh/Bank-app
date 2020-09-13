using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebbApp.ViewModels
{
    public class StatisticsViewModel
    {
        public int NumberOfCustomers { get; set; }
        public int NumberOfAccounts { get; set; }
        public decimal SumofAccountsBalances{ get; set; }

    }
}
