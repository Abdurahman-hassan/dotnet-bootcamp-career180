using ATMApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Entities
{
    public class InternalTransfer
    {
        public decimal TransferAmount { get; set; }

        public long RecipientBankAccountNumber { get; set; }
        public string RecipientBankAccountName { get; set; }
        // Sender's info and transfer status
        public TransferStatus Status { get; set; }= TransferStatus.Pending;
        public long SenderAccountNumber{ get; set; }
        public string SenderAccountName { get; set; }
        

    }
}
