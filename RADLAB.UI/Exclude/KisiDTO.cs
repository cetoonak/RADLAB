using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KisiDTO
    {
        public int Id { get; set; }

        [Range(1, 9999999999, ErrorMessage = "Bu alanı doldurmalısınız")]
        public int KurulusId { get; set; }
        public string Kurulus { get; set; } = string.Empty;

        //[Range(1, 9999999999, ErrorMessage = "Bu alanı doldurmalısınız")]
        public int HastaneId { get; set; }
        public string Hastane { get; set; } = string.Empty;

        [Range(1, 9999999999, ErrorMessage = "Bu alanı doldurmalısınız")]
        public int BirimId { get; set; }
        public string Birim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Soyad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string TCKimlikNo { get; set; } = string.Empty;

        [Range(1, 9999999999, ErrorMessage = "Bu alanı doldurmalısınız\"")]
        public int KimlikTuru { get; set; }
        public string KimlikTuruString { get; set; } = string.Empty;

        public int MeslekId { get; set; }
        public string Meslek { get; set; } = string.Empty;

        public int CalismaAlaniBirimId { get; set; }
        public string CalismaAlaniBirim { get; set; } = string.Empty;

        public int VucutBolgesiId1 { get; set; }
        public string VucutBolgesi1 { get; set; } = string.Empty;

        public int VucutBolgesiId2 { get; set; }
        public string VucutBolgesi2 { get; set; } = string.Empty;

        public string Durum { get; set; } = string.Empty;

        public List<LookupBasicDTO> VucutBolgeleri { get; set; } = new List<LookupBasicDTO>();

        public List<LookupBasicDTO> Birimler { get; set; } = new List<LookupBasicDTO>();

        public int SonlandirilanDonem { get; set; }

        public bool Sorumlu { get; set; }

        public bool Aktif { get; set; }

        public bool Checked { get; set; }
    }
}