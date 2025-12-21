using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class SoruService : ISoruService
    {
        private readonly HttpClient httpClient;

        public SoruService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<List<SoruDTO>>> GetSoruList(SoruFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Soru/GetSoruList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<SoruDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateSoru(SoruDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Soru/InsertOrUpdateSoru", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteSoru(List<SoruDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Soru/DeleteSoru", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}