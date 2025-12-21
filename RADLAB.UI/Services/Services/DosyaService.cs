using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class DosyaService : IDosyaService
    {
        private readonly HttpClient httpClient;

        public DosyaService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<DosyaDTO>> GetDosya(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<DosyaDTO>>("Dosya/GetDosya/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<DosyaDTO>>> GetDosyaList()
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<DosyaDTO>>>("Dosya/GetDosyaList");
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateDosya(DosyaDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Dosya/InsertOrUpdateDosya", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteDosya(List<DosyaDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Dosya/DeleteDosya", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}