namespace RADLAB.Model.DTO
{
    public class LoginDTO
    {
        public string Mail { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Password2 { get; set; } = string.Empty;
        public string AktivasyonKodu { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public bool TokenAl { get; set; }
        public int LoginTipi { get; set; } // 1: Sistem kullanıcısı, 2: Online eğitim kullanıcısı
    }
}