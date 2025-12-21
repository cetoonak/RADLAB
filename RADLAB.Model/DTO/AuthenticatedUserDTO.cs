namespace RADLAB.Model.DTO
{
    public class AuthenticatedUserDTO
    {
        public string Mail { get; set; } = string.Empty;
        public string AdSoyad { get; set; } = string.Empty;
        public string Unvan { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string TelefonCep { get; set; } = string.Empty;
        public List<MenuBaslikDTO> MenuBasliklar { get; set; } = new List<MenuBaslikDTO>();
        public List<MenuLinkDTO> MenuLinkler { get; set; } = new List<MenuLinkDTO>();
        public List<string> Yetkiler { get; set; } = new List<string>();
    }
}