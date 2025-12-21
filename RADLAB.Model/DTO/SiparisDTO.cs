using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class SiparisDTO
    {
        public int Id { get; set; }
        public string GSM { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string AdSoyadSiparis { get; set; } = string.Empty;
        public string TCKimlikNoSiparis { get; set; } = string.Empty;
        public int IlIdSiparis { get; set; }
        public string IlSiparis { get; set; } = string.Empty;
        public int IlceIdSiparis { get; set; }
        public string IlceSiparis { get; set; } = string.Empty;
        public string AdresSiparis { get; set; } = string.Empty;
        public int FaturaTipi { get; set; }
        public string AdSoyadFatura { get; set; } = string.Empty;
        public string TCKimlikNoFatura { get; set; } = string.Empty;
        public string UnvanFatura { get; set; } = string.Empty;
        public string VergiDairesiFatura { get; set; } = string.Empty;
        public string VergiNoFatura { get; set; } = string.Empty;
        public int IlIdFatura { get; set; }
        public string IlFatura { get; set; } = string.Empty;
        public int IlceIdFatura { get; set; }
        public string IlceFatura { get; set; } = string.Empty;
        public string AdresFatura { get; set; } = string.Empty;
        public int KargoFirmasiId { get; set; }
        public string KargoFirmasi { get; set; } = string.Empty;
        public decimal KargoUcreti { get; set; }
        public DateTime? SiparisZamani { get; set; }
        public string KrediKartiAdSoyad { get; set; } = string.Empty;
        public string KrediKartiNo { get; set; } = string.Empty;
        public string KrediKartiSonKullanimYil { get; set; } = string.Empty;
        public string KrediKartiSonKullanimAy { get; set; } = string.Empty;
        public string KrediKartiCVV { get; set; } = string.Empty;
        public string KrediKartiBrandName { get; set; } = string.Empty;
        public bool SozlesmeleriKabulEtti { get; set; }
        public List<SiparisCihazDTO> SiparisCihazList { get; set; }
        public string SiparisDurumuString { get; set; } = string.Empty;
        public string VerifyEnrollmentRequestId { get; set; } = string.Empty;
        public int TahsilatSekliId { get; set; }
        public string PosIslemSonucu { get; set; } = string.Empty;
        public bool Expanded { get; set; }
        public int DetailCount { get; set; }
        public bool Checked { get; set; }
        public decimal Tutar { get; set; }
    }
}