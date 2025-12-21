using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class MailDTO
    {
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public List<string> ToList { get; set; }
        public string GSM { get; set; } = string.Empty;
        public string Mesaj { get; set; } = string.Empty;
        public bool KendisineDeGonder { get; set; } = false;
    }
}