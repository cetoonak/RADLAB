using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class TanimBasicService : ITanimBasicService
    {
        private readonly HttpClient httpClient;

        public TanimBasicService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<TanimBasicDTO> GetTanimBasic(string Tanim, int Id)
        {
            return await httpClient.GetFromJsonAsync<TanimBasicDTO>("TanimBasic/GetTanimBasic/" + Tanim + "/" + Id.ToString());
        }

        public async Task<List<TanimBasicDTO>> GetTanimBasicList(string Tanim)
        {
            return await httpClient.GetFromJsonAsync<List<TanimBasicDTO>>("TanimBasic/GetTanimBasicList/" + Tanim);
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateTanimBasic(string Tanim, TanimBasicDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("TanimBasic/InsertOrUpdateTanimBasic/" + Tanim, dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteTanimBasic(string Tanim, List<TanimBasicDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("TanimBasic/DeleteTanimBasic/" + Tanim, dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}