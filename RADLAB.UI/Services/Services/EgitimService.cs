using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class EgitimService : IEgitimService
    {
        private readonly HttpClient httpClient;

        public EgitimService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<EgitimDTO> GetEgitim(int Id)
        {
            return await httpClient.GetFromJsonAsync<EgitimDTO>("Egitim/GetEgitim/" + Id.ToString());
        }

        public async Task<List<EgitimDTO>> GetEgitimList()
        {
            return await httpClient.GetFromJsonAsync<List<EgitimDTO>>("Egitim/GetEgitimList");
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateEgitim(EgitimDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Egitim/InsertOrUpdateEgitim", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteEgitim(List<EgitimDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Egitim/DeleteEgitim", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}