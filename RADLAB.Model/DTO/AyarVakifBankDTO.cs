using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class AyarVakifBankDTO
    {
        public string VakifBankMerchantId { get; set; } = string.Empty;
        public string VakifBankMerchantPassword { get; set; } = string.Empty;
        public string VakifTerminalNo { get; set; } = string.Empty;
        public string VakifBankEnrollmentUrl { get; set; } = string.Empty;
    }
}