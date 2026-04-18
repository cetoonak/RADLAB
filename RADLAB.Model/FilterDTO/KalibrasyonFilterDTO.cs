namespace RADLAB.Model.FilterDTO
{
    public class KalibrasyonFilterDTO : FilterDTO
    {
        public string AdSoyadUnvan { get; set; } = string.Empty;
        public string BasvuruTakipNo { get; set; } = string.Empty;
        public string BasvuruTarihiAraligi { get; set; } = string.Empty;
        public int BasvuruTarihi1 { get; set; }
        public int BasvuruTarihi2 { get; set; }
        public string Cihaz { get; set; } = string.Empty;
        public int GelisSekli { get; set; }
        public int KalibrasyonOdeme { get; set; }
        public string Aciklama { get; set; } = string.Empty;
        public string Sayfa { get; set; } = string.Empty;
        public string Order { get; set; } = string.Empty;
        public int Id { get; set; }
    }
}