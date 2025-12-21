using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace RADLAB.Model.DTO
{
    public class RolDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Rol adını giriniz")]
        public string Rol { get; set; } = string.Empty;

        public List<YetkiDTO> Yetkiler { get; set; } = new List<YetkiDTO>();

        public bool Checked { get; set; }
    }
}