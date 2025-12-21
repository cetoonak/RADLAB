using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;
using System.Net.Http.Json;

namespace RADLAB.UI.Services.Services
{
    public class AyarService : IAyarService
    {
        private readonly HttpClient httpClient;

        public AyarService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<AyarDTO> GetAyar()
        {
            return await httpClient.GetFromJsonAsync<AyarDTO>("Ayar/GetAyar");
        }

        public async Task<AyarDTO> GetAyarForOlcum()
        {
            return await httpClient.GetFromJsonAsync<AyarDTO>("Ayar/GetAyarForOlcum");
        }

        public async Task<AyarKargoDTO> GetAyarForKargo(KargoAyarFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Ayar/GetAyarForKargo", filterDTO);

            return await result.Content.ReadFromJsonAsync<AyarKargoDTO>();
        }

        public async Task<string> GetMetin(string field)
        {
            return await httpClient.GetStringAsync($"Ayar/GetMetin/{field}");
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateAyar(AyarDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Ayar/InsertOrUpdateAyar", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}