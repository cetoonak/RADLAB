using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class CihazDTO
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Seviye { get; set; }
        public string Icon { get; set; } = string.Empty;
        public string Cihaz { get; set; } = string.Empty;
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public int CihazTuruId { get; set; }
        public string CihazTuru { get; set; } = string.Empty;
        public int Miktar { get; set; }
        public decimal Fiyat { get; set; }
        public decimal KalibrasyonUcreti { get; set; }
        public decimal KdvOrani { get; set; }
        public int KritikStok { get; set; }
        public float Desi { get; set; }
        public int StokMiktari { get; set; }
        public string IdAcilimi { get; set; } = string.Empty;
        public string AdAcilimi { get; set; } = string.Empty;
        public string Ozellik { get; set; } = string.Empty;
        public List<CihazResimDTO> CihazResimleri { get; set; }
        public string ResimDosyaAdi { get; set; } = string.Empty;
        public string ResimUzanti { get; set; } = string.Empty;
        public string ResimDosya { get; set; } = string.Empty;
        public string KullanmaKlavuzuDosyaAdi { get; set; } = string.Empty;
        public string KullanmaKlavuzuUzanti { get; set; } = string.Empty;
        public string KullanmaKlavuzuIcerik { get; set; } = string.Empty;
    }
}