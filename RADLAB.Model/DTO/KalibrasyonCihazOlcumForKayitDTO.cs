using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazOlcumForKayitDTO
    {
        public KalibrasyonCihazDTO KalibrasyonCihaz { get; set; }
        public List<KalibrasyonCihazOlcumDTO> KalibrasyonCihazOlcumList { get; set; }
        public HareketTuruDTO HareketTuru { get; set; }
    }
}