using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace RADLAB.Model.DTO
{
    public class AyarDTO
    {
        public string KurumAdi { get; set; } = string.Empty;
        public string Adres { get; set; } = string.Empty;
        public string Telefon { get; set; } = string.Empty;
        public string Faks { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public string WebAdresi { get; set; } = string.Empty;
        public string VergiDairesi { get; set; } = string.Empty;
        public string VergiNo { get; set; } = string.Empty;
        public string Banka { get; set; } = string.Empty;
        public string IBAN { get; set; } = string.Empty;
        public int KisiIdLaboratuvarSorumlusu { get; set; }
        public int KisiIdKurumSorumlusu { get; set; }
        public int KargoFirmasiId { get; set; }
        public int KargoBedavaLimitliDesili { get; set; }
        public decimal KargoBedavaLimiti { get; set; }
        public float KargoDesi1 { get; set; }
        public float KargoDesi2 { get; set; }
        public float KargoDesi3 { get; set; }
        public float KargoDesi4 { get; set; }
        public float KargoDesi5 { get; set; }
        public decimal KargoDesiUcret1 { get; set; }
        public decimal KargoDesiUcret2 { get; set; }
        public decimal KargoDesiUcret3 { get; set; }
        public decimal KargoDesiUcret4 { get; set; }
        public decimal KargoDesiUcret5 { get; set; }
        public string EMailSunucusu { get; set; } = string.Empty;
        public string EMailGonderenAdres { get; set; } = string.Empty;
        public string EMailGonderenSifre { get; set; } = string.Empty;
        public int EMailPort { get; set; }
        public bool EMailSSL { get; set; }
        public float DamgaVergisiOrani { get; set; }
        public float SozlesmePuluOrani { get; set; }
        public DateTime? ReferansTarihi { get; set; }
        public float Sicaklik1 { get; set; }
        public float Sicaklik2 { get; set; }
        public float Basinc1 { get; set; }
        public float Basinc2 { get; set; }
        public float Nem1 { get; set; }
        public float Nem2 { get; set; }
        public float DDozCarpimDozHizi1 { get; set; }
	    public float DDozCarpimDozHizi2 { get; set; }
	    public float DDozCarpimDozHizi3 { get; set; }
	    public float DDozCarpimDozHizi4 { get; set; }
	    public float DDozCarpimDozHizi5 { get; set; }
	    public float DDozCarpimToplamDoz { get; set; }
	    public float DDozCikarilanDozHizi1 { get; set; }
	    public float DDozCikarilanDozHizi2 { get; set; }
	    public float DDozCikarilanDozHizi3 { get; set; }
	    public float DDozCikarilanDozHizi4 { get; set; }
	    public float DDozCikarilanDozHizi5 { get; set; }
	    public float DDozCikarilanToplamDoz { get; set; }
        public string ResimAntetUst { get; set; } = string.Empty;
        public string ResimAntetAlt { get; set; } = string.Empty;
        public string ResimLogo { get; set; } = string.Empty;
        public string ResimTurkak { get; set; } = string.Empty;
        public string RadyoaktifKaynakYuklemeMetni { get; set; } = string.Empty;
        public string GizlilikSozlesmesiMetni { get; set; } = string.Empty;
        public string OnBilgilendirmeFormuMetni { get; set; } = string.Empty;
        public string MesafeliSatisSozlesmesiMetni { get; set; } = string.Empty;
        public string IadeSozlesmesiMetni { get; set; } = string.Empty;
        public string MailSablonu { get; set; } = string.Empty;
    }
}