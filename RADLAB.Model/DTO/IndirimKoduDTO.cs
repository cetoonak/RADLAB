using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class IndirimKoduDTO
    {
        public int Id { get; set; }
        public string IndirimKodu { get; set; } = string.Empty;
        public string GSM { get; set; } = string.Empty;
        public decimal Tutar { get; set; }
        public string AdSoyadUnvan { get; set; } = string.Empty;
        public string BasvuruTakipNo { get; set; } = string.Empty;
    }
}