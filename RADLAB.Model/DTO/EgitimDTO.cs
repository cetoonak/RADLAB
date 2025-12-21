using System.ComponentModel.DataAnnotations;

namespace RADLAB.Model.DTO
{
    public class EgitimDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int Sira { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Egitim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Baslik { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Kisaltma { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public int NormalGunluk { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Amac { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Sart { get; set; } = string.Empty;
    }
}