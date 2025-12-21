using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IKalibrasyonManager
    {
        //////////////////////////////////////////////// Kalibrasyon ////////////////////////////////////////////
        public Task<ServiceResponse<KalibrasyonDTO>> GetKalibrasyon(int Id, int KisiId);
        public Task<ServiceResponse<List<KalibrasyonDTO>>> GetKalibrasyonList(KalibrasyonFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyon(KalibrasyonDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteKalibrasyon(List<int> Idler, int KisiId);
        
        //////////////////////////////////////////////// KalibrasyonCihaz ////////////////////////////////////////////
        public Task<ServiceResponse<KalibrasyonCihazDTO>> GetKalibrasyonCihaz(int Id, int KisiId);
        public Task<ServiceResponse<List<KalibrasyonCihazDTO>>> GetKalibrasyonCihazList(KalibrasyonCihazFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<List<KalibrasyonCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihaz(KalibrasyonCihazDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteKalibrasyonCihaz(List<int> Idler, int KisiId);

        //////////////////////////////////////////// KalibrasyonCihazHareket ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazHareketDTO>>> GetKalibrasyonCihazHareketList(KalibrasyonCihazHareketFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto, int KisiId);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteKalibrasyonCihazHareket(List<int> Idler, int KisiId);

        //////////////////////////////////////////// KalibrasyonCihazOdeme ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazOdemeDTO>>> GetKalibrasyonCihazOdemeList(KalibrasyonCihazOdemeFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazOdeme(KalibrasyonCihazOdemeDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteKalibrasyonCihazOdeme(List<int> Idler, int KisiId);

        //////////////////////////////////////////// KalibrasyonCihazHareketDosya ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>> GetKalibrasyonCihazHareketDosyaList(int Id, int KisiId);

        //////////////////////////////////////////// KalibrasyonCihazOlcum ////////////////////////////////////////////
        public Task<ServiceResponse<List<KalibrasyonCihazOlcumDTO>>> GetKalibrasyonCihazOlcumList(int Id, int KisiId);
        public Task<ServiceResponse<string>> UpdateKalibrasyonCihazOlcum(KalibrasyonCihazOlcumForKayitDTO dto, int KisiId);

        //////////////////////////////////////////// Turkak ////////////////////////////////////////////
        public Task<ServiceResponse<string>> TurkakaGonder(KalibrasyonCihazDTO dto, int KisiId);
        public Task<ServiceResponse<string>> TurkakQRKoduAl(KalibrasyonCihazDTO dto, int KisiId);
    }
}