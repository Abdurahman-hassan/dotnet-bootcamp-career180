using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATMApp.Domain.Entities
{
    public class UserAccount
    {
        public long CardNumber { get; set; }
        public int CardPin { get; set; }
        public long AccountNumber { get; set; }
        public string FullName { get; set; } 
        public decimal AccountBalance { get; set; }
        public int TotalLogin { get; set; }
        public bool isLocked    { get; set; }
        public int Id { get; set; }

    }
}
