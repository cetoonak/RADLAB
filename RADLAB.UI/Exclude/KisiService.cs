using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System.Net.Http.Json;

namespace RADLAB.UI.Exclude
{
    public class KisiService : IKisiService
    {
        private readonly HttpClient httpClient;

        public KisiService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<KisiDTO> GetKisi(int Id)
        {
            return await httpClient.GetFromJsonAsync<KisiDTO>("Kisi/GetKisi/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<KisiDTO>>> GetKisiList(KisiFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/GetKisiList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KisiDTO>>>();
        }

        public async Task<List<LookupBasicDTO>> GetVucutBolgesiListByKisiId(int KisiId)
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Kisi/GetVucutBolgesiListByKisiId/" + KisiId.ToString());
        }

        public async Task<List<LookupBasicDTO>> GetBirimListByKisiId(int KisiId)
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Kisi/GetBirimListByKisiId/" + KisiId.ToString());
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateKisi(KisiDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/InsertOrUpdateKisi", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKisi(List<KisiDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/DeleteKisi", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/UpdateKisiProfil", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<KisiOnayDTO>>> GetKisiOnayList(KisiOnayFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/GetKisiOnayList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KisiOnayDTO>>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiOnayAcikRiza(List<KisiOnayDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/UpdateKisiOnayAcikRiza", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/UpdateKisiCookiePolitikasiniGordu", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiAcikRiza(KullaniciForLoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/UpdateKisiAcikRiza", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiGizlilikOnaylandi(KullaniciForLoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kisi/UpdateKisiGizlilikOnaylandi", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}