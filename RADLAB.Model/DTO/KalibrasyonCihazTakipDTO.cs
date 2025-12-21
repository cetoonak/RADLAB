using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazTakipDTO
    {
        public int Id { get; set; }
        public int KalibrasyonId { get; set; }
        public string BasvuruTakipNo { get; set; } = string.Empty;
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SeriNo { get; set; } = string.Empty;
        public string CihazDurumu { get; set; } = string.Empty;
        public string SonIslemZamani { get; set; } = string.Empty;
        public decimal OdenecekTutar { get; set; }
    }
}