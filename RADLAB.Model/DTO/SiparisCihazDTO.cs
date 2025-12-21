using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class SiparisCihazDTO
    {
        public int Id { get; set; }
        public int SiparisId { get; set; }
        public int MarkaId { get; set; }
        public int ModelId { get; set; }
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SeriNo { get; set; } = string.Empty;
        public int Miktar { get; set; }
        public decimal Fiyat { get; set; }
        public decimal KdvOrani { get; set; }
        public DateTime? SonIslemZamani { get; set; }
        public string Durum { get; set; } = string.Empty;
        public string Badge { get; set; } = string.Empty;
        public List<LookupBasicDTO> DataSourceModel { get; set; }
        public bool Checked { get; set; }
    }
}