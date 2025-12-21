using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class StokHareketleriFilterDTO : FilterDTO
    {
        public int MarkaId { get; set; }
        public int ModelId { get; set; }
        public int StokHareketTuruId { get; set; }
        public string HareketTarihiAraligi { get; set; } = string.Empty;
        public int HareketTarihi1 { get; set; }
        public int HareketTarihi2 { get; set; }
        public string Order { get; set; } = string.Empty;
    }
}