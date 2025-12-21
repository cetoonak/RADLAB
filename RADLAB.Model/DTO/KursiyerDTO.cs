using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KursiyerDTO
    {
        public int Id { get; set; }
        public int KursId { get; set; }
        public int Sira { get; set; }
        public string TCKimlikNo { get; set; } = string.Empty;
        public string Ad { get; set; } = string.Empty;
        public string Soyad { get; set; } = string.Empty;
        public DateTime? DogumTarihi { get; set; }
        public int Cinsiyet { get; set; }
        public string CinsiyetString { get; set; } = string.Empty;
        public string Mail { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public string Meslek { get; set; } = string.Empty;
        public string Okul { get; set; } = string.Empty;
        public string Diploma { get; set; } = string.Empty;
        public string DiplomaDosyaAdi { get; set; } = string.Empty;
        public string DiplomaUzanti { get; set; } = string.Empty;
        public string KimlikOn { get; set; } = string.Empty;
        public string KimlikOnDosyaAdi { get; set; } = string.Empty;
        public string KimlikOnUzanti { get; set; } = string.Empty;
        public string KimlikArka { get; set; } = string.Empty;
        public string KimlikArkaDosyaAdi { get; set; } = string.Empty;
        public string KimlikArkaUzanti { get; set; } = string.Empty;
        public string Aciklama { get; set; } = string.Empty;
        public int BeklemedeAsilYedekIptal { get; set; }
        public string BeklemedeAsilYedekIptalString { get; set; } = string.Empty;
        public decimal Fiyat { get; set; }
        public string Firma { get; set; } = string.Empty;
        public string VergiDairesi { get; set; } = string.Empty;
        public string VergiNo { get; set; } = string.Empty;
        public string AdresFirma { get; set; } = string.Empty;
        public int IlIdAdres { get; set; }
        public int IlIlceIdAdres { get; set; }
        public int IlIdFirma { get; set; }
        public int IlIlceIdFirma { get; set; }
        public string IlFirma { get; set; } = string.Empty;
        public string IlceFirma { get; set; } = string.Empty;
        public int IlIdTercih { get; set; }
        public string IlTercih { get; set; } = string.Empty;
        public string DekontNo { get; set; } = string.Empty;
        public string Dekont { get; set; } = string.Empty;
        public string DekontDosyaAdi { get; set; } = string.Empty;
        public string DekontUzanti { get; set; } = string.Empty;
        public string BasvuruTakipNo { get; set; } = string.Empty;
    }
}