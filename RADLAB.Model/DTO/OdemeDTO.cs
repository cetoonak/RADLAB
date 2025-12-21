using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class OdemeDTO
    {
        public string Id { get; set; } = string.Empty;
        public int TabloId { get; set; }
        public string Tablo { get; set; } = string.Empty;
        public string OdemeTuru { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public DateTime? TahakkukTarihi { get; set; }
        public decimal OdenecekTutar { get; set; }
        public string VerifyEnrollmentRequestId { get; set; } = string.Empty;
        public string CVV { get; set; } = string.Empty;
        public int TahsilatSekliId { get; set; }
        public string PosIslemSonucu { get; set; } = string.Empty;
        public string AdSoyadSiparis { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string GSM { get; set; } = string.Empty;
        public string AdresSiparis { get; set; } = string.Empty;
        public string AdresFatura { get; set; } = string.Empty;
        public decimal KargoUcreti { get; set; }
        public bool Checked { get; set; }
    }
}