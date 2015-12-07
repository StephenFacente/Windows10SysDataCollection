using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows10SystemDataCollector.DataTypes
{
    public class EventsLog
    {
        public string Message { get; set; }
        public string Source { get; set; }
        public DateTime Time { get; set; }
    }
}
