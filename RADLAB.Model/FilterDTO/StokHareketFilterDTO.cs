using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class StokHareketFilterDTO : FilterDTO
    {
        public int StokHareketTuruId { get; set; }
        public string EvrakNo { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public string Cihaz { get; set; } = string.Empty;
        public string HareketTarihiAraligi { get; set; } = string.Empty;
        public int HareketTarihi1 { get; set; }
        public int HareketTarihi2 { get; set; }
        public string Order { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}