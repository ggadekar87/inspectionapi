using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BankApp.Data.Models
{
    public class AccountType
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public ICollection<BankAccount> Accounts { get; set; } = new List<BankAccount>();

    }
}
