using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KullaniciDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad giriniz")]
        public string Ad { get; set; } = string.Empty;

        [Required(ErrorMessage = "Soyad giriniz")]
        public string Soyad { get; set; } = string.Empty;

        public string TelefonCep { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "E-Posta adresi giriniz")]
        public string EMail { get; set; } = string.Empty;

        public bool Aktif { get; set; }

        public bool SMSKalibrasyon { get; set; }
        
        public bool SMSEgitim { get; set; }
        
        public bool SMSSiparis { get; set; }

        public List<RolDTO> Roller { get; set; } = new List<RolDTO>();
        
        public bool Checked { get; set; }
    }
}