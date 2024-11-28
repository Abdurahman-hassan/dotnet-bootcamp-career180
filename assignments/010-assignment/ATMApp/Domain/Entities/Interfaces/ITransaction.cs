using ATMApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Entities.Interfaces
{
    public interface ITransaction
    {
        void InsertTransaction(long _UserBankAccountID, TransactionType _TransType, decimal _TransAmount, string _Description);
        void ViewTransaction();
    }
}
