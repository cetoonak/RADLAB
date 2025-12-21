using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class AyarKargoDTO
    {
        public int KargoFirmasiId { get; set; }
        public string KargoFirmasi { get; set; } = string.Empty;
        public decimal KargoUcreti { get; set; }
    }
}