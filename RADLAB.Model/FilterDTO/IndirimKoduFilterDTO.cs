using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class IndirimKoduFilterDTO : FilterDTO
    {
        public string IndirimKodu { get; set; } = string.Empty;
        public string AdSoyadUnvan { get; set; } = string.Empty;
        public string BasvuruTakipNo { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}