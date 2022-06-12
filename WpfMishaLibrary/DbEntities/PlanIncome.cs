using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.DbEntities
{
    public class PlanIncome
    {
        public int PlaInId { get; set; }
        public string IncomeCategory { get; set; }
        public double Sum { get; set; }
        // Goes to the db as the number of Ticks (int)!!!
        public DateTime BeginDate { get; set; }
        // Goes to the db as the number of Ticks (int)!!!
        public DateTime EndDate { get; set; }
        public string PlanIncomeImagePath { get; set; }
    }
}
