using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazOdemeDTO
    {
        public int Id { get; set; }
        public int KalibrasyonCihazId { get; set; }
        public int OdemeTuruId { get; set; }
        public string OdemeTuru { get; set; } = string.Empty;
        public decimal Ucret { get; set; }
        public DateTime? TahakkukTarihi { get; set; }
        public DateTime? OdemeTarihi { get; set; }
    }
}