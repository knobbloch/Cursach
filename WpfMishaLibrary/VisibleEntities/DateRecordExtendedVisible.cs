using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.VisibleEntities
{
    public abstract class DateRecordExtendedVisible
    {
        // Store date in db as INT format. Return date as DateTime and long.
        public DateTime BeginDateDateTime { get; set; }
        public DateTime EndDateDateTime { get; set; }
    }
}
