using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazHareketDosyaDTO : DosyaDTO
    {
        public int Id { get; set; }
        public int KalibrasyonCihazHareketId { get; set; }   
    }
}
