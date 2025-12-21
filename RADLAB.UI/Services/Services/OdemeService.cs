using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class OdemeService : IOdemeService
    {
        private readonly HttpClient httpClient;

        public OdemeService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<List<OdemeDTO>>> GetOdeme(MusteriTakipFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Odeme/GetOdeme", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<OdemeDTO>>>();
        }

        public async Task<ServiceResponse<List<OdemeDTO>>> GetOdemeByVerifyEnrollmentRequestId(string VerifyEnrollmentRequestId)
        {
            var result = await httpClient.PostAsJsonAsync("Odeme/GetOdemeByVerifyEnrollmentRequestId", VerifyEnrollmentRequestId);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<OdemeDTO>>>();
        }

        public async Task<ServiceResponse<string>> UpdateOdeme(List<OdemeDTO> dto)
        {
            var result = await httpClient.PostAsJsonAsync("Odeme/UpdateOdeme", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateOdemeVerifyEnrollmentRequestId(List<OdemeDTO> dto)
        {
            var result = await httpClient.PostAsJsonAsync("Odeme/UpdateOdemeVerifyEnrollmentRequestId", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}