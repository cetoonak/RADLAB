using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class SiparisCihazTakipDTO
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public DateTime? SiparisZamani { get; set; }
        public string AdSoyadSiparis { get; set; } = string.Empty;
        public string SiparisAdresi { get; set; } = string.Empty;
        public string FaturaAdresi { get; set; } = string.Empty;
        public string SiparisDurumu { get; set; } = string.Empty;
        public string Urun { get; set; } = string.Empty;
        public decimal Fiyat { get; set; }
        public DateTime? SonIslemZamani { get; set; }
        public string SiparisCihazDurumu { get; set; } = string.Empty;
        public string SiparisCihazDurumuBadge { get; set; } = string.Empty;
        public bool IptalEdilebilir { get; set; }
        public bool IadeEdilebilir { get; set; }
    }
}