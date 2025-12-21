using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class KursService : IKursService
    {
        private readonly HttpClient httpClient;

        public KursService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<List<KursDTO>>> GetKursList(KursFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/GetKursList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KursDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateKurs(KursDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/InsertOrUpdateKurs", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKurs(List<KursDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/DeleteKurs", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<KursYayinDTO>>> GetKursYayinList(int Id)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/GetKursYayinList", Id);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KursYayinDTO>>>();
        }

        public async Task<ServiceResponse<List<KursiyerTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/GetMusteriTakip", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<KursiyerTakipDTO>>>();
        }

        public async Task<ServiceResponse<KursiyerDTO>> GetKursiyer(int Id)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/GetKursiyer", Id);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<KursiyerDTO>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateKursiyer(KursiyerDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/InsertOrUpdateKursiyer", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteKursiyer(List<KursiyerDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Kurs/DeleteKursiyer", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}