using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System.Net.Http;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IKalibrasyonService
    {
        //////////////////////////////////////////////// Kalibrasyon ////////////////////////////////////////////
        public Task<ServiceResponse<KalibrasyonDTO>> GetKalibrasyon(int Id);
        public Task<ServiceResponse<List<KalibrasyonDTO>>> GetKalibrasyonList(KalibrasyonFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyon(KalibrasyonDTO dto);
        public Task<ServiceResponse<string>> DeleteKalibrasyon(List<int> Idler);

        //////////////////////////////////////////////// KalibrasyonCihaz ////////////////////////////////////////////
        public Task<ServiceResponse<KalibrasyonCihazDTO>> GetKalibrasyonCihaz(int Id);
        public Task<ServiceResponse<List<KalibrasyonCihazDTO>>> GetKalibrasyonCihazList(KalibrasyonCihazFilterDTO filterDTO);
        public Task<ServiceResponse<List<KalibrasyonCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihaz(KalibrasyonCihazDTO dto);
        public Task<ServiceResponse<string>> DeleteKalibrasyonCihaz(List<int> Idler);

        //////////////////////////////////////////// KalibrasyonCihazHareket ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazHareketDTO>>> GetKalibrasyonCihazHareketList(KalibrasyonCihazHareketFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto);
        public Task<ServiceResponse<string>> DeleteKalibrasyonCihazHareket(List<int> Idler);

        //////////////////////////////////////////// KalibrasyonCihazOdeme ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazOdemeDTO>>> GetKalibrasyonCihazOdemeList(KalibrasyonCihazOdemeFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazOdeme(KalibrasyonCihazOdemeDTO dto);
        public Task<ServiceResponse<string>> DeleteKalibrasyonCihazOdeme(List<int> Idler);

        //////////////////////////////////////////// KalibrasyonCihazHareketDosya ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>> GetKalibrasyonCihazHareketDosyaList(int Id);

        //////////////////////////////////////////// KalibrasyonCihazOlcum ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazOlcumDTO>>> GetKalibrasyonCihazOlcumList(int Id);
        public Task<ServiceResponse<string>> UpdateKalibrasyonCihazOlcum(KalibrasyonCihazOlcumForKayitDTO dto);

        //////////////////////////////////////////// Turkak ////////////////////////////////////////////
        public Task<ServiceResponse<string>> TurkakaGonder(KalibrasyonCihazDTO dto);
        public Task<ServiceResponse<string>> TurkakQRKoduAl(KalibrasyonCihazDTO dto);
    }
}