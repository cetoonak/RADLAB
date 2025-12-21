using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Model.DTO
{
    public class KalibrasyonCihazOlcumDTO
    {
        public int Id { get; set; }
        public int KalibrasyonCihazId { get; set; }
        public int DozHiziToplamDoz { get; set; }
        public int Sira { get; set; }
        public decimal Uzaklik { get; set; }
        public decimal Sure { get; set; }
        public decimal DiskSayisi { get; set; }
        public decimal DDoz { get; set; }
        public decimal BozulmaDuzelme { get; set; }
		public decimal OlcumNicelik { get; set; }
		public decimal ReferansDeger { get; set; }
		public int OlcuBirimiId { get; set; }
		public string OlcuBirimi { get; set; } = string.Empty;
        public decimal ReferansBelirsizlik { get; set; }
		public decimal Olcum1 { get; set; }
		public decimal Olcum2 { get; set; }
		public decimal Olcum3 { get; set; }
		public decimal Olcum4 { get; set; }
		public decimal Olcum5 { get; set; }
		public decimal Olcum6 { get; set; }
		public decimal Olcum7 { get; set; }
		public decimal Olcum8 { get; set; }
		public decimal Olcum9 { get; set; }
		public decimal Olcum10 { get; set; }
		public decimal StandartSapma { get; set; }
		public decimal MusteriBelirsizligi { get; set; }
		public decimal MusteriCihazi { get; set; }
		public decimal Sapma { get; set; }
		public decimal ToplamBelirsizlik { get; set; }
    }
}