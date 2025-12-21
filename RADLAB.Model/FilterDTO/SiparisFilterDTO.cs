using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class SiparisFilterDTO : FilterDTO
    {
        public string GSM { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string AdSoyadSiparis { get; set; } = string.Empty;
        public string TCKimlikNoSiparis { get; set; } = string.Empty;
        public int IlId { get; set; }
        public string SiparisTarihiAraligi { get; set; } = string.Empty;
        public int SiparisTarihi1 { get; set; }
        public int SiparisTarihi2 { get; set; }
        public int SiparisDurumu { get; set; }
        public string Order { get; set; } = string.Empty;
        public int Id { get; set; }
        public string VerifyEnrollmentRequestId { get; set; } = string.Empty;
    }
}