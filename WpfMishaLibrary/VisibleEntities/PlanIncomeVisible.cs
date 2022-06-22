using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.VisibleEntities
{
    public class PlanIncomeVisible: DateRecordExtendedVisible
    {
        public string IncomeCategory { get; set; }
        public double Sum { get; set; }
        public string PlanIncomeImagePath { get; set; }
    }
}
