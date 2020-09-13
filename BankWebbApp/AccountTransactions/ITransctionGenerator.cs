using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankWebbApp.AccountTransactions
{
  public interface ITransctionGenerator
    {
        bool ValidateOperation();
        void SaveTransaction();
    }
}
