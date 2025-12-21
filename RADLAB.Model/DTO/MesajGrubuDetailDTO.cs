using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajGrubuDetailDTO
    {
        public int Id { get; set; }
        public int MesajGrubuMasterId { get; set; }
        public int UyeKisiId { get; set; }
        public string UyeKisi { get; set; } = string.Empty;
    }
}