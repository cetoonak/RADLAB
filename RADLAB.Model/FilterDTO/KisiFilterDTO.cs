using RADLAB.Model.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.FilterDTO
{
    public class KisiFilterDTO : FilterDTO
    {
        public int KurulusId { get; set; }
        public int BirimId { get; set; }
        public string AdSoyad { get; set; } = string.Empty;
        public string TCKimlikNo { get; set; } = string.Empty;
        public int KimlikTuru { get; set; }
        public int MeslekId { get; set; }
        public int CalismaAlaniBirimId { get; set; }
        public int VucutBolgesiId { get; set; }
        public int SonlandirilanDonem { get; set; }
        public int Sorumlu { get; set; }
        public int Aktif { get; set; }
    }
}