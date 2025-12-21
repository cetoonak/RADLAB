using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class CihazResimDTO
    {
        public int Id { get; set; }
        public int CihazId { get; set; }
        public string DosyaAdi { get; set; } = string.Empty;
        public string Uzanti { get; set; } = string.Empty;
        public string DosyaBuyuk { get; set; } = string.Empty;
        public string DosyaKucuk { get; set; } = string.Empty;
    }
}