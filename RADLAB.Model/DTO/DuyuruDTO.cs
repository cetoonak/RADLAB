using System.ComponentModel.DataAnnotations;

namespace RADLAB.Model.DTO
{
    public class DuyuruDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public DateTime? Tarih { get; set; }

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Konu { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alanı doldurmalısınız")]
        public string Icerik { get; set; } = string.Empty;

        public bool SistemKullanicilarinaGoster { get; set; }
        
        public bool WebSitesindeGoster { get; set; }
        
        public bool DozimetreKulanicilarinaGoster { get; set; }
        
        public bool Aktif { get; set; }
    }
}