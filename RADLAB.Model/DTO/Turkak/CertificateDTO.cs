namespace RADLAB.Model.DTO.Turkak
{
    public class CertificateDTO
    {
        public string ID { get; set; } = string.Empty;
        public string CustomerID { get; set; } = string.Empty;
        public DateTime FirstReleaseDateOfTheDocument { get; set; }
        public string MachineOrDeviceType { get; set; } = string.Empty;
        public string DeviceSerialNumber { get; set; } = string.Empty;
        public string PersonnelPerformingCalibration { get; set; } = string.Empty;
        public string CalibrationLocation { get; set; } = string.Empty;
        public string SerialNumberOfReferenceCalibrator { get; set; } = string.Empty;
        public DateTime CalibrationDate { get; set; }
        public DateTime? RevisionDate { get; set; }
        public string RevisionNote { get; set; } = string.Empty;
    }
}