using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.VisibleEntities;

namespace WpfMishaLibrary.DbEntities
{
    public class FactIncome: FactIncomeVisible
    {
        public int FactIncomeCategoryId { get; set; }
        public int CardId { get; set; }
        // Goes to the db as the number of Ticks (int)!!!
        public long Date { get; set; }
    }
}
