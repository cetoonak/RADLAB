using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class TanimSayiliService : ITanimSayiliService
    {
        private readonly HttpClient httpClient;

        public TanimSayiliService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<TanimSayiliDTO> GetTanimSayili(string Tanim, int Id)
        {
            return await httpClient.GetFromJsonAsync<TanimSayiliDTO>("TanimSayili/GetTanimSayili/" + Tanim + "/" + Id.ToString());
        }

        public async Task<List<TanimSayiliDTO>> GetTanimSayiliList(string Tanim)
        {
            return await httpClient.GetFromJsonAsync<List<TanimSayiliDTO>>("TanimSayili/GetTanimSayiliList/" + Tanim);
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateTanimSayili(string Tanim, TanimSayiliDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("TanimSayili/InsertOrUpdateTanimSayili/" + Tanim, dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteTanimSayili(string Tanim, List<TanimSayiliDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("TanimSayili/DeleteTanimSayili/" + Tanim, dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}