namespace RADLAB.Model.DTO.Turkak
{
    public class CustomerDTO
    {
        public string ID { get; set; } = string.Empty;
        public string CountryID { get; set; } = string.Empty;
        public string CityID { get; set; } = string.Empty;
        public string FileID { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public bool UseTitleFromTaxNumber { get; set; }
        public string BrandInformation { get; set; } = string.Empty;
        public string TaxNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string EMail { get; set; } = string.Empty;
        public int DVAccountType { get; set; }
        public string UpdateNote { get; set; } = string.Empty;
    }
}