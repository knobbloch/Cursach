using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.VisibleEntities
{
    public class FactExpenditureVisible: DateRecordVisible
    {
        public string ExpenditureName { get; set; }
        public string FactExpenditureCategory { get; set; }
        public double Sum { get; set; }
        public string CardName { get; set; }
    }
}
