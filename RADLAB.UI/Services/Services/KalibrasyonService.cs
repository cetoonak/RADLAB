using Microsoft.AspNetCore.Mvc;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class KalibrasyonService : IKalibrasyonService
    {
        private readonly HttpClient httpClient;

        public KalibrasyonService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        //////////////////////////////////////////////// Kalibrasyon ////////////////////////////////////////////

        public async Task<ServiceResponse<KalibrasyonDTO>> GetKalibrasyon(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<KalibrasyonDTO>>("Kalibrasyon/GetKalibrasyon/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<KalibrasyonDTO>>> GetKalibrasyonList(KalibrasyonFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetKalibrasyonList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyon(KalibrasyonDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/InsertUpdateKalibrasyon", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKalibrasyon(List<int> Idler)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/DeleteKalibrasyon", Idler);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        //////////////////////////////////////////////// KalibrasyonCihaz ////////////////////////////////////////////

        public async Task<ServiceResponse<KalibrasyonCihazDTO>> GetKalibrasyonCihaz(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<KalibrasyonCihazDTO>>("Kalibrasyon/GetKalibrasyonCihaz/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<KalibrasyonCihazDTO>>> GetKalibrasyonCihazList(KalibrasyonCihazFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetKalibrasyonCihazList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonCihazDTO>>>();
        }

        public async Task<ServiceResponse<List<KalibrasyonCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetMusteriTakip", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonCihazTakipDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihaz(KalibrasyonCihazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/InsertUpdateKalibrasyonCihaz", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihaz(List<int> Idler)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/DeleteKalibrasyonCihaz", Idler);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        //////////////////////////////////////////// KalibrasyonCihazHareket ////////////////////////////////////////////

        public async Task<ServiceResponse<string>> InsertKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/InsertKalibrasyonCihazHareket", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<KalibrasyonCihazHareketDTO>>> GetKalibrasyonCihazHareketList(KalibrasyonCihazHareketFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetKalibrasyonCihazHareketList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonCihazHareketDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazHareket(KalibrasyonCihazHareketDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/InsertUpdateKalibrasyonCihazHareket", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihazHareket(List<int> Idler)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/DeleteKalibrasyonCihazHareket", Idler);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        //////////////////////////////////////////// KalibrasyonCihazOdeme ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazOdemeDTO>>> GetKalibrasyonCihazOdemeList(KalibrasyonCihazOdemeFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetKalibrasyonCihazOdemeList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonCihazOdemeDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertUpdateKalibrasyonCihazOdeme(KalibrasyonCihazOdemeDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/InsertUpdateKalibrasyonCihazOdeme", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKalibrasyonCihazOdeme(List<int> Idler)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/DeleteKalibrasyonCihazOdeme", Idler);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        //////////////////////////////////////////// KalibrasyonCihazHareketDosya ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>> GetKalibrasyonCihazHareketDosyaList(int Id)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetKalibrasyonCihazHareketDosyaList", Id);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonCihazHareketDosyaDTO>>>();
        }

        //////////////////////////////////////////// KalibrasyonCihazOlcum ////////////////////////////////////////////

        public async Task<ServiceResponse<List<KalibrasyonCihazOlcumDTO>>> GetKalibrasyonCihazOlcumList(int Id)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/GetKalibrasyonCihazOlcumList", Id);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KalibrasyonCihazOlcumDTO>>>();
        }

        public async Task<ServiceResponse<string>> UpdateKalibrasyonCihazOlcum(KalibrasyonCihazOlcumForKayitDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/UpdateKalibrasyonCihazOlcum", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        //////////////////////////////////////////// Turkak ////////////////////////////////////////////

        public async Task<ServiceResponse<string>> TurkakaGonder(KalibrasyonCihazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/TurkakaGonder", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> TurkakQRKoduAl(KalibrasyonCihazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kalibrasyon/TurkakQRKoduAl", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}