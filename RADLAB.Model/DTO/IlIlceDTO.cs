using System.ComponentModel.DataAnnotations;

namespace RADLAB.Model.DTO
{
    public class IlIlceDTO
    {
        public int Id { get; set; }

        public int ParentId { get; set; }

        [Required(ErrorMessage = "Bu alan gerekli")]
        public string IlIlce { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli")]
        public string Kod { get; set; } = string.Empty;

        public int Seviye { get; set; }

        public string Icon { get; set; } = string.Empty;

        public string IdAcilimi { get; set; } = string.Empty;

        public bool EgitimYapilanIl { get; set; }
    }
}