using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.VisibleEntities
{
    public class PlanExpenditureVisible: DateRecordExtendedVisible
    {
        public string ExpenditureCategory { get; set; }
        public double Sum { get; set; }
        public string PlanExpenditureImagePath { get; set; }
    }
}
