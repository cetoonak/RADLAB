using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KursiyerTakipDTO
    {
        public int Id { get; set; }
        public string Egitim { get; set; } = string.Empty;
        public string BasvuruDonemi { get; set; } = string.Empty;
        public string EgitimDonemi { get; set; } = string.Empty;
        public string KursDurumu { get; set; } = string.Empty;
        public string KursiyerDurumu { get; set; } = string.Empty;
        public decimal OdenecekTutar { get; set; }
    }
}