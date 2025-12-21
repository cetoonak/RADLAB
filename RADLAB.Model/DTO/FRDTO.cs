namespace RADLAB.Model.DTO
{
    public class FRDTO
    {
        public string ReportPath { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string ReportNameOrBase64 { get; set; } = string.Empty;
        public string Uzanti { get; set; } = string.Empty;

        public string MusteriBilgisi { get; set; } = string.Empty;
        public int KisiId { get; set; }
        public int Id { get; set; }
        public int Donem { get; set; }
        public string Donemler { get; set; } = string.Empty;
        public int KurulusId { get; set; }
        public int BirimId { get; set; }
        public string BirimIdler { get; set; } = string.Empty;
        public int VucutBolgesiId { get; set; }
        public string VucutBolgesiIdler { get; set; } = string.Empty;
        public int Yil { get; set; }
        public string DozFirmasiIdler { get; set; } = string.Empty;
        public int KurulusTuruId { get; set; }
        public int AdresTekCift { get; set; }
        public int SonucTuru { get; set; }
        public int KisiToplam { get; set; }
        public int TumuAktifPasif { get; set; }
        public int PageSize { get; set; }
        public bool Antetli { get; set; }
        public bool AyriAyri { get; set; }
        public bool Ortak { get; set; }
        public bool Barkodlu { get; set; }
        public bool AktifleriGoster { get; set; }
        public bool YeniTalepleriGoster { get; set; }
        public bool DevirleriGoster { get; set; }
        public bool SonlanmislariGoster { get; set; }
        public bool KullanildilariGoster { get; set; }
        public bool PasifleriGoster { get; set; }
        public bool DagitimYapilmamislariGoster { get; set; }
        public bool AciklamaGoster { get; set; }
        public string DagitimMasterIdler { get; set; } = string.Empty;
        public string TCKimlikNo { get; set; } = string.Empty;
        public string Barkod { get; set; } = string.Empty;
        public int BosSatirSayisi { get; set; }
        public string DozimetreNo { get; set; } = string.Empty;
        public int KacGunOnceden { get; set; }
        public DateTime? Tarih1 { get; set; }
        public DateTime? Tarih2 { get; set; }


        //public List<FRParameter> Parameters { get; set; }

        public List<FRParameter> Parameters => new List<FRParameter>()
        {
            new FRParameter() { Name = "MusteriBilgisi", Value = MusteriBilgisi},
            new FRParameter() { Name = "KisiId", Value = KisiId.ToString()},
            new FRParameter() { Name = "Id", Value = Id.ToString()},
            new FRParameter() { Name = "Donem", Value = Donem.ToString()},
            new FRParameter() { Name = "Donemler", Value = Donemler},
            new FRParameter() { Name = "KurulusId", Value = KurulusId.ToString()},
            new FRParameter() { Name = "BirimId", Value = BirimId.ToString()},
            new FRParameter() { Name = "BirimIdler", Value = BirimIdler == "" ? "0" : BirimIdler},
            new FRParameter() { Name = "VucutBolgesiId", Value = VucutBolgesiId.ToString()},
            new FRParameter() { Name = "VucutBolgesiIdler", Value = VucutBolgesiIdler == "" ? "0" : VucutBolgesiIdler},
            new FRParameter() { Name = "Yil", Value = Yil.ToString()},
            new FRParameter() { Name = "DozFirmasiIdler", Value = DozFirmasiIdler == "" ? "0" : DozFirmasiIdler},
            new FRParameter() { Name = "KurulusTuruId", Value = KurulusTuruId.ToString()},
            new FRParameter() { Name = "AdresTekCift", Value = AdresTekCift.ToString()},
            new FRParameter() { Name = "SonucTuru", Value = SonucTuru.ToString()},
            new FRParameter() { Name = "KisiToplam", Value = KisiToplam.ToString()},
            new FRParameter() { Name = "TumuAktifPasif", Value = TumuAktifPasif.ToString()},
            new FRParameter() { Name = "PageSize", Value = PageSize.ToString()},
            new FRParameter() { Name = "Antetli", Value = Antetli ? "1" : "0"},
            new FRParameter() { Name = "AyriAyri", Value = AyriAyri ? "1" : "0"},
            new FRParameter() { Name = "Ortak", Value = Ortak ? "1" : "0"},
            new FRParameter() { Name = "Barkodlu", Value = Barkodlu ? "1" : "0"},
            new FRParameter() { Name = "AktifleriGoster", Value = AktifleriGoster ? "1" : "0"},
            new FRParameter() { Name = "YeniTalepleriGoster", Value = YeniTalepleriGoster ? "1" : "0"},
            new FRParameter() { Name = "DevirleriGoster", Value = DevirleriGoster ? "1" : "0"},
            new FRParameter() { Name = "SonlanmislariGoster", Value = SonlanmislariGoster ? "1" : "0"},
            new FRParameter() { Name = "KullanildilariGoster", Value = KullanildilariGoster ? "1" : "0"},
            new FRParameter() { Name = "PasifleriGoster", Value = PasifleriGoster ? "1" : "0"},
            new FRParameter() { Name = "DagitimYapilmamislariGoster", Value = DagitimYapilmamislariGoster ? "1" : "0"},
            new FRParameter() { Name = "AciklamaGoster", Value = AciklamaGoster ? "1" : "0"},
            new FRParameter() { Name = "DagitimMasterIdler", Value = DagitimMasterIdler == "" ? "0" : DagitimMasterIdler},
            new FRParameter() { Name = "TCKimlikNo", Value = TCKimlikNo},
            new FRParameter() { Name = "Barkod", Value = Barkod},
            new FRParameter() { Name = "BosSatirSayisi", Value = BosSatirSayisi.ToString()},
            new FRParameter() { Name = "DozimetreNo", Value = DozimetreNo == "" ? "0" : DozimetreNo},
            new FRParameter() { Name = "KacGunOnceden", Value = KacGunOnceden.ToString()},
            new FRParameter() { Name = "Tarih1", Value = Tarih1.HasValue ? Tarih1.Value.ToOADate().ToString() : "0"},
            new FRParameter() { Name = "Tarih2", Value = Tarih2.HasValue ? Tarih2.Value.ToOADate().ToString() : "0"}
        };

        public FRDTO ()
        {
            //var configurationBuilder = new ConfigurationBuilder();
            //string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            //configurationBuilder.AddJsonFile(path, false);
            //string connectionString = configurationBuilder.Build().GetSection("ConnectionStrings:Default").Value;

            //ConnectionString = connectionString;

            ReportPath = System.IO.Directory.GetCurrentDirectory() + "\\FR\\";

            //Parameters = new List<FRParameter> ();
        }
    }
    public class FRParameter
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}