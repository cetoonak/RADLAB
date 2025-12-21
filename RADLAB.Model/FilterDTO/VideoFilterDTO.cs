using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class VideoFilterDTO : FilterDTO
    {
        public int KlasorId { get; set; }
        public string DosyaAdi { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}