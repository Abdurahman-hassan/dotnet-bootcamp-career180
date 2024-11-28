using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 5. Modify AppMenu.cs to add new menu option
namespace ATMApp.Domain.Enums
{
    public enum AppMenu
    {
        CheckBalance = 1,
        PlaceDeposit,
        MakeWithdrawal,
        InternalTransfer,
        ViewTransactions,
        ViewPendingTransfers,  
        Logout
    }
}