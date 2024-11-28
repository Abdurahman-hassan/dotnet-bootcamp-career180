using ATMApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Entities
{
    public class PendingTransfer
    {
        public long transferID {  get; set; }
        public long SenderAccountName {  get; set; }
        public long SenderAccountNumber { get; set; }
        public long RecipientAccountNumber { get; set; }
        public long RecipientAccountName { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate{ get; set; }
        public TransferStatus TransferStatus { get; set; }  
    }
}
