using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfMishaLibrary.DbEntities;

namespace WpfMishaLibrary.VisibleEntities
{
    public abstract class DateRecordVisible
    {
        // Store date in db as INT format. Return date as DateTime and long.
        public DateTime DateDateTime { get; set; }   
    }
}
