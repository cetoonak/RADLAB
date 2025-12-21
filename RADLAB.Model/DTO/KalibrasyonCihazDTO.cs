using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazDTO
    {
        public int Id { get; set; }
        public int KalibrasyonId { get; set; }
        public int Sira { get; set; }
        public string BasvuruTakipNo { get; set; } = string.Empty;
        public int MarkaId { get; set; }
        public int ModelId { get; set; }
        public string Marka { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string SeriNo { get; set; } = string.Empty;
        public decimal KalibrasyonUcreti { get; set; }
        public decimal KdvOrani { get; set; }
        public string CihazDurumu { get; set; } = string.Empty;
        public DateTime? TeslimTarihi { get; set; }
        public bool OdemeYapildi { get; set; }
        public List<LookupBasicDTO> DataSourceModel { get; set; }
        public DateTime? ReferansTarihi { get; set; }
        public DateTime? KalibrasyonTarihi { get; set; }
        public DateTime? GonderilisTarihi { get; set; }
        public DateTime? YayinlandigiTarih { get; set; }
        public string SonIslemZamani { get; set; } = string.Empty;
        public decimal Bozunma { get; set; }
        public int DozHiziTuruId { get; set; }
        public string DozHiziTuru { get; set; } = string.Empty;
        public int ToplamDozTuruId { get; set; }
        public string ToplamDozTuru { get; set; } = string.Empty;
        public decimal Sicaklik1 { get; set; }
        public decimal Sicaklik2 { get; set; }
        public decimal Basinc1 { get; set; }
        public decimal Basinc2 { get; set; }
        public decimal Nem1 { get; set; }
        public decimal Nem2 { get; set; }
        public decimal ReferansDoz { get; set; }
        public decimal DDozCarpimDozHizi1 { get; set; }
        public decimal DDozCarpimDozHizi2 { get; set; }
        public decimal DDozCarpimDozHizi3 { get; set; }
        public decimal DDozCarpimDozHizi4 { get; set; }
        public decimal DDozCarpimDozHizi5 { get; set; }
        public decimal DDozCarpimToplamDoz { get; set; }
        public decimal DDozCikarilanDozHizi1 { get; set; }
        public decimal DDozCikarilanDozHizi2 { get; set; }
        public decimal DDozCikarilanDozHizi3 { get; set; }
        public decimal DDozCikarilanDozHizi4 { get; set; }
        public decimal DDozCikarilanDozHizi5 { get; set; }
        public decimal DDozCikarilanToplamDoz { get; set; }
        public decimal ToplamBelirsizlikSiniri { get; set; }
        public int SonucTuruId { get; set; }
        public string SonucTuru { get; set; } = string.Empty;
        public string SonucAciklama { get; set; } = string.Empty;
        public int AciklamaSayisi { get; set; }
        public string TurkakCustomerGuid { get; set; } = string.Empty;
        public string TurkakCertificateGuid { get; set; } = string.Empty;
        public string TurkakTBDSNumber { get; set; } = string.Empty;
        public string TurkakCertificationBodyDocumentNumber { get; set; } = string.Empty;
        public DateTime? TurkakaGonderilmeZamani { get; set; }
        public bool Checked { get; set; }
    }
}