using RADLAB.Model.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonDTO
    {
        public int Id { get; set; }

        //[Range(1, 2, ErrorMessage = "Bu alan gerekli")]
        public int BasvuruTipi { get; set; }

        //[Range(1, 3, ErrorMessage = "Bu alan gerekli")]
        public int FaturaTipi { get; set; }

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[EmailAddress(ErrorMessage = "E-posta adresi formatı hatalı")]
        public string Mail { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("9999999999", ErrorMessage = "Telefon numarası 0 ile başlamayan 10 karakter olmalıdır")]
        public string TelefonCep { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string AdSoyadBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string AdSoyadFatura { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("99999999999", ErrorMessage = "TC Kimlik numarası 11 karakter olmalıdır")]
        public string TCKimlikNoBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("99999999999", ErrorMessage = "TC Kimlik numarası 11 karakter olmalıdır")]
        public string TCKimlikNoFatura { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string UnvanBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string UnvanFatura { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string VergiDairesiBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string VergiDairesiFatura { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("9999999999", ErrorMessage = "Vergi numarası 10 karakter olmalıdır")]
        public string VergiNoBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("9999999999", ErrorMessage = "Vergi numarası 10 karakter olmalıdır")]
        public string VergiNoFatura { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int IlIdBasvuru { get; set; }
        public string IlBasvuru { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int IlIdFatura { get; set; }
        public string IlFatura { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int IlIdTeslim { get; set; }
        public string IlTeslim { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int IlceIdBasvuru { get; set; }
        public string IlceBasvuru { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int IlceIdFatura { get; set; }
        public string IlceFatura { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int IlceIdTeslim { get; set; }
        public string IlceTeslim { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("99999", ErrorMessage = "Posta kodu 5 karakter olmalıdır")]
        public string PostaKoduBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("99999", ErrorMessage = "Posta kodu 5 karakter olmalıdır")]
        public string PostaKoduFatura { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        //[PhoneMask("99999", ErrorMessage = "Posta kodu 5 karakter olmalıdır")]
        public string PostaKoduTeslim { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string AdresBasvuru { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string AdresFatura { get; set; } = string.Empty;

        //[Required(ErrorMessage = "Bu alan gerekli")]
        public string AdresTeslim { get; set; } = string.Empty;

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int TeslimAdresiTipi { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Bu alan gerekli")]
        public int GelisSekli { get; set; }
        public string GelisSekliString { get; set; } = string.Empty;
        public string GelisSekliBadge { get; set; } = string.Empty;

        public DateTime BasvuruZamani { get; set; }
        public string KalibrasyonDurumu { get; set; } = string.Empty;
        public string BasvuruTakipNo { get; set; } = string.Empty;

        public List<KalibrasyonCihazDTO> KalibrasyonCihazList { get; set; }
        public string Cihazlar { get; set; } = string.Empty;

        public string AdSoyadUnvan { get; set; } = string.Empty;

        public int KalibrasyonOdeme { get; set; }

        public int IndirimKoduId { get; set; }
        public string IndirimKodu { get; set; } = string.Empty;
        public decimal IndirimTutari { get; set; }

        public int DetailCount { get; set; }

        public bool Expanded { get; set; }
        public bool Checked { get; set; }
    }
}