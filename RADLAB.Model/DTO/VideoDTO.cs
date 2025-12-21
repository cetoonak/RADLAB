using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class VideoDTO
    {
        public int Id { get; set; }
        public int KlasorId { get; set; }
        public string Klasor { get; set; } = string.Empty;
        public string DosyaAdi { get; set; } = string.Empty;
        public string Uzanti { get; set; } = string.Empty;
        public string Dosya { get; set; } = string.Empty;
    }
}