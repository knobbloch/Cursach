using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.VisibleEntities;

namespace WpfMishaLibrary.DbEntities
{
    public class PlanIncome: PlanIncomeVisible
    {
        public int PlanIncomeId { get; set; }
        // Goes to the db as the number of Ticks (int)!!!
        public long BeginDate { get; set; }
        public long EndDate { get; set; }
    }
}
