using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KursYayinDTO
    {
        public int Id { get; set; }
        public string Egitim { get; set; } = string.Empty;
        public string Baslik { get; set; } = string.Empty;
        public string Kisaltma { get; set; } = string.Empty;
        public string Amac { get; set; } = string.Empty;
        public string Sart { get; set; } = string.Empty;
        public string KursOrtami { get; set; } = string.Empty;
        public string BasvuruDonemi { get; set; } = string.Empty;
        public string EgitimDonemi { get; set; } = string.Empty;
        public string Sure { get; set; } = string.Empty;
        public string KursYeri { get; set; } = string.Empty;
        public string Durum { get; set; } = string.Empty;
    }
}