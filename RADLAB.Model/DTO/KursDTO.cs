using System.ComponentModel.DataAnnotations;

namespace RADLAB.Model.DTO
{
    public class KursDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int EgitimId { get; set; }
        public string Egitim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int Sira { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int KursOrtamiId { get; set; }
        public string KursOrtami { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public DateTime? BasvuruBaslangici { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public DateTime? BasvuruBitisi { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public DateTime? EgitimBaslangici { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public DateTime? EgitimBitisi { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public DateTime? YayindanKalkis { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Sure { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public decimal Fiyat { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int KdvOraniId { get; set; }
        public string KdvOrani { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int KontenjanAsil { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int KontenjanYedek { get; set; }

        public string Aciklama { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string KursYeri { get; set; } = string.Empty;

        public string Durum { get; set; } = string.Empty;
        public string Badge { get; set; } = string.Empty;
        public int Beklemede { get; set; }
        public int Asil { get; set; }
        public int Yedek { get; set; }
        public int Iptal { get; set; }
        public int Toplam { get; set; }

        public List<KursiyerDTO> KursiyerList { get; set; }

        public int DetailCount { get; set; }

        public bool Expanded { get; set; }
    }
}