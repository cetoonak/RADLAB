using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class KalibrasyonFilterDTO : FilterDTO
    {
        public string AdSoyadUnvan { get; set; } = string.Empty;
        public string BasvuruTakipNo { get; set; } = string.Empty;
        public string BasvuruTarihiAraligi { get; set; } = string.Empty;
        public int BasvuruTarihi1 { get; set; }
        public int BasvuruTarihi2 { get; set; }
        public string Cihaz { get; set; } = string.Empty;
        public int GelisSekli { get; set; }
        public int KalibrasyonOdeme { get; set; }
        public string Sayfa { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}