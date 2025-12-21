using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class KalibrasyonCihazFilterDTO : FilterDTO
    {
        public int Id { get; set; }
        public int KalibrasyonId { get; set; }
        public string BasvuruTakipNo { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public int MarkaId { get; set; }
        public int ModelId { get; set; }
        public string SeriNo { get; set; } = string.Empty;
        public string Sayfa { get; set; } = string.Empty;
    }
}