using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO.Turkak
{
    public class CertificateForNumbersDTO
    {
        public string ID { get; set; } = string.Empty;
        public string CustomerID { get; set; } = string.Empty;
        public string TBDSNumber { get; set; } = string.Empty;
        public string CertificationBodyDocumentNumber { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
    }
}