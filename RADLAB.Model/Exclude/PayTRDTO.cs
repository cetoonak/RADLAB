using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RADLAB.Model.DTO;

namespace RADLAB.Model.Exclude
{
    public class PayTRDTO
    {
        public int Id { get; set; }
        public string emailstr { get; set; } = string.Empty;
        public int payment_amountstr { get; set; }
        public string merchant_oid { get; set; } = string.Empty;
        public List<UserBasketDTO> UserBasketList { get; set; }
        public string user_name { get; set; } = string.Empty;
        public string user_address { get; set; } = string.Empty;
        public string user_phone { get; set; } = string.Empty;
    }
}