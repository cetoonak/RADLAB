namespace RADLAB.Model.DTO
{
    public class MenuLinkDTO
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public string Ad { get; set; } = string.Empty;
        public string Link { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public bool Favori { get; set; }
    }
}