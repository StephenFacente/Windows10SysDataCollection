using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Windows10SystemDataCollector.DataTypes
{
    public class USBInfoData
    {
        public string Name { get; set; }
        //public string HwId { get; set; }
        public string Guid { get; set; }
        public string Description { get; set; }
        public string ContainerID { get; set; }
    }
}
