using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.DbEntities
{
    public class PlanExpenditure
    {
        public string ExpenditureCategory { get; set; }
        public double Sum { get; set; }
        // Goes to the db as the number of Ticks (int)!!!
        public long BeginDate { get; set; }
        // Goes to the db as the number of Ticks (int)!!!
        public long EndDate { get; set; }
        public DateTime BeginDateDateTime { get; set; }
        public DateTime EndDateDateTime { get; set; }
        public string PlanExpenditureImagePath { get; set; }
    }
}
