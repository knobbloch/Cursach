using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfMishaLibrary.DbEntities
{
    public abstract class Record
    {
        // Store date in db as INT format. Return date as DateTime and long.
        public DateTime DateDateTime;
        public long Date;
    }
}
