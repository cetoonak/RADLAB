using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class VakifBankMPIDTO
    {
        public string Status { get; set; } = string.Empty;
        public string ErrorCode { get; set; } = string.Empty;
        public string ACSUrl { get; set; } = string.Empty;
        public string PAReq { get; set; } = string.Empty;
        public string TermUrl { get; set; } = string.Empty;
        public string MD { get; set; } = string.Empty;
        public string PostBackForm { get; set; } = string.Empty;
    }
}