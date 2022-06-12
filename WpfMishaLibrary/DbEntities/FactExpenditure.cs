using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.DbEntities
{
    public class FactExpenditure: Record
    {
        public string ExpenditureName { get; set; }
        public string FactExpenditureCategory { get; set; }
        public double Sum { get; set; }
        // Date stored with INT value in the database!!! Storing Ticks.
        public long Date { get; set; }
        public DateTime DateDateTime { get; set; }
        public string CardName { get; set; }
    }
}
