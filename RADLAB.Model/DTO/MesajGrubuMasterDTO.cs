using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MesajGrubuMasterDTO
    {
        public int Id { get; set; }
        //public int OlusturanKisiId { get; set; }
        //public string OlusturanKisi { get; set; } = string.Empty;
        public string GrupAdi { get; set; } = string.Empty;
        public string Kisiler { get; set; } = string.Empty;
        public List<MesajGrubuDetailDTO> MesajGrubuDetailList { get; set; }
    }
}