using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class SiparisService : ISiparisService
    {
        private readonly HttpClient httpClient;

        public SiparisService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<List<SiparisDTO>>> GetSiparisList(SiparisFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/GetSiparisList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<SiparisDTO>>>();
        }

        public async Task<ServiceResponse<List<SiparisDTO>>> GetSiparisListByVerifyEnrollmentRequestId(string VerifyEnrollmentRequestId)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/GetSiparisListByVerifyEnrollmentRequestId", VerifyEnrollmentRequestId);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<SiparisDTO>>>();
        }

        public async Task<ServiceResponse<List<SiparisCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/GetMusteriTakip", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<SiparisCihazTakipDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertSiparis(SiparisDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/InsertSiparis", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateSiparis(SiparisDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/InsertOrUpdateSiparis", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteSiparis(List<SiparisDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/DeleteSiparis", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateSiparisCihaz(SiparisCihazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/InsertOrUpdateSiparisCihaz", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteSiparisCihaz(List<SiparisCihazDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/DeleteSiparisCihaz", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<SiparisCihazHareketDTO>>> GetSiparisCihazHareket(int SiparisCihazId)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/GetSiparisCihazHareket", SiparisCihazId);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<SiparisCihazHareketDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertSiparisCihazHareket(SiparisCihazHareketDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Siparis/InsertSiparisCihazHareket", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}