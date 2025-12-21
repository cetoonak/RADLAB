using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.OnlineEgitim.Services.Infrastructure;

namespace RADLAB.OnlineEgitim.Services.Services
{
    public class KullaniciService : IKullaniciService
    {
        private readonly HttpClient httpClient;

        public KullaniciService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<KullaniciDTO> GetKullanici(int Id)
        {
            return await httpClient.GetFromJsonAsync<KullaniciDTO>("Kullanici/GetKullanici/" + Id.ToString());
        }

        public async Task<List<KullaniciDTO>> GetKullaniciList()
        {
            return await httpClient.GetFromJsonAsync<List<KullaniciDTO>>("Kullanici/GetKullaniciList");
        }

        public async Task<List<RolDTO>> GetRolListVerilenByKullaniciId(int KullaniciId)
        {
            return await httpClient.GetFromJsonAsync<List<RolDTO>>("Kullanici/GetRolListVerilenByKullaniciId/" + KullaniciId.ToString());
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateKullanici(KullaniciDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kullanici/InsertOrUpdateKullanici", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKullanici(List<KullaniciDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Kullanici/DeleteKullanici", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kullanici/UpdateKisiProfil", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kullanici/UpdateKisiCookiePolitikasiniGordu", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}