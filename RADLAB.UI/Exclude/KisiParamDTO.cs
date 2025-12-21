using RADLAB.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.ParamDTO
{
    public class KisiParamDTO : KisiDTO
    {
        public string VucutBolgesiIdler { get; set; } = string.Empty;
        public string BirimIdler { get; set; } = string.Empty;
    }
}