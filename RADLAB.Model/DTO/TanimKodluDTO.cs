using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class TanimKodluDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bu alan gerekli")]
        public string Tanim { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bu alan gerekli")]
        public string Kod { get; set; } = string.Empty;
    }
}