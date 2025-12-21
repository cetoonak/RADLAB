using RADLAB.Model.DTO;
using RADLAB.Model.Exclude;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System.Net.Http.Json;

namespace RADLAB.UI.Exclude
{
    public class PayTRService : IPayTRService
    {
        private readonly HttpClient httpClient;

        public PayTRService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<string>> PayTRAdim1(PayTRDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("PayTR/PayTRAdim1", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> PayTRAdim2(PayTRDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("PayTR/PayTRAdim2", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateSiparisPayTRToken(SiparisDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("PayTR/UpdateSiparisPayTRToken", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateSiparisPayTROdemeTamam(string MerchantOID)
        {
            var result = await httpClient.PostAsJsonAsync("PayTR/UpdateSiparisPayTROdemeTamam", MerchantOID);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<string> GetSiparisPayTRToken(int id)
        {
            return await httpClient.GetStringAsync($"PayTR/GetSiparisPayTRToken/{id}");
        }
    }
}