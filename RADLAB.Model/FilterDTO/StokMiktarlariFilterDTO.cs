using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class StokMiktarlariFilterDTO : FilterDTO
    {
        public int MarkaId { get; set; }
        public int ModelId { get; set; }
        public string Order { get; set; } = string.Empty;
    }
}