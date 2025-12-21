using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class TanimKodluService : ITanimKodluService
    {
        private readonly HttpClient httpClient;

        public TanimKodluService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<TanimKodluDTO> GetTanimKodlu(string Tanim, int Id)
        {
            return await httpClient.GetFromJsonAsync<TanimKodluDTO>("TanimKodlu/GetTanimKodlu/" + Tanim + "/" + Id.ToString());
        }

        public async Task<List<TanimKodluDTO>> GetTanimKodluList(string Tanim)
        {
            return await httpClient.GetFromJsonAsync<List<TanimKodluDTO>>("TanimKodlu/GetTanimKodluList/" + Tanim);
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateTanimKodlu(string Tanim, TanimKodluDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("TanimKodlu/InsertOrUpdateTanimKodlu/" + Tanim, dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteTanimKodlu(string Tanim, List<TanimKodluDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("TanimKodlu/DeleteTanimKodlu/" + Tanim, dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}