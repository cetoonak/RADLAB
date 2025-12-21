using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class DuyuruService : IDuyuruService
    {
        private readonly HttpClient httpClient;

        public DuyuruService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<DuyuruDTO> GetDuyuru(int Id)
        {
            return await httpClient.GetFromJsonAsync<DuyuruDTO>("Duyuru/GetDuyuru/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<DuyuruDTO>>> GetDuyuruList(DuyuruFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Duyuru/GetDuyuruList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<DuyuruDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateDuyuru(DuyuruDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Duyuru/InsertOrUpdateDuyuru", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteDuyuru(List<DuyuruDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Duyuru/DeleteDuyuru", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}