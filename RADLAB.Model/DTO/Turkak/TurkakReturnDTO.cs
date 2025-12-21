using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO.Turkak
{
    public class TurkakReturnDTO
    {
        public List<ReturnItemDTO> Item1 { get; set; }
        public List<ReturnItemDTO> Item2 { get; set; }
    }

    public class ReturnItemDTO
    {
        public string ID { get; set; } = string.Empty;
        public string ErrorDescription { get; set; } = string.Empty;
    }
}