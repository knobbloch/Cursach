using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.DbEntities;
namespace WpfMishaLibrary.VisibleEntities
{
    public class FactIncomeVisible: DateRecordVisible
    {
        public string FactIncomeName { get; set; }
        public string FactIncomeCategory { get; set; }
        public string CategoryImagePath { get; set; }
        public double Sum { get; set; }
        public string CardName { get; set; }
    }
}
