using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class SiparisCihazHareketDTO
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public int SiparisCihazId { get; set; }
        public int SiparisCihazHareketTuruId { get; set; }
        public string SiparisCihazHareketTuru { get; set; } = string.Empty;
        public DateTime? IslemZamani { get; set; }
        public int KargoFirmasiId { get; set; }
        public string KargoFirmasi { get; set; } = string.Empty;
        public string KargoTakipNo { get; set; } = string.Empty;
        public string FaturaNo { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public bool SozlesmeleriKabulEtti { get; set; }
    }
}