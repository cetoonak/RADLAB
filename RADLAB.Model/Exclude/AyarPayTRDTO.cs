using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.Exclude
{
    public class AyarPayTRDTO
    {
        public string PayTRMerchantId { get; set; } = string.Empty;
        public string PayTRMerchantKey { get; set; } = string.Empty;
        public string PayTRMerchantSalt { get; set; } = string.Empty;
    }
}