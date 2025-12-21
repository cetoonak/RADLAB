using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class IndirimKoduService : IIndirimKoduService
    {
        private readonly HttpClient httpClient;

        public IndirimKoduService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<List<IndirimKoduDTO>>> GetIndirimKoduList(IndirimKoduFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("IndirimKodu/GetIndirimKoduList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<IndirimKoduDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateIndirimKodu(IndirimKoduDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("IndirimKodu/InsertOrUpdateIndirimKodu", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteIndirimKodu(List<IndirimKoduDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("IndirimKodu/DeleteIndirimKodu", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}