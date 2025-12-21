using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class OnlineEgitimService : IOnlineEgitimService
    {
        private readonly HttpClient httpClient;

        public OnlineEgitimService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<OnlineEgitimDTO>> GetOnlineEgitim(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<OnlineEgitimDTO>>("OnlineEgitim/GetOnlineEgitim/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<OnlineEgitimDTO>>> GetOnlineEgitimList(OnlineEgitimFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/GetOnlineEgitimList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<OnlineEgitimDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateOnlineEgitim(OnlineEgitimDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/InsertOrUpdateOnlineEgitim", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteOnlineEgitim(List<OnlineEgitimDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/DeleteOnlineEgitim", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> InsertOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/InsertOnlineEgitimOgrenci", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/DeleteOnlineEgitimOgrenci", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<List<OnlineEgitimTreeDTO>>> GetOnlineEgitimTreeList(int OnlineEgitimOgrenciId)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<OnlineEgitimTreeDTO>>>("OnlineEgitim/GetOnlineEgitimTreeList/" + OnlineEgitimOgrenciId.ToString());
        }

        public async Task<ServiceResponse<string>> InsertUpdateOnlineEgitimBolumTamamlanan(OnlineEgitimBolumTamamlananDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/InsertUpdateOnlineEgitimBolumTamamlanan", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananGecenSure(OnlineEgitimTreeDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/UpdateOnlineEgitimBolumTamamlananGecenSure", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<int>> GetOnlineEgitimBolumTamamlananGecenSure(int OnlinEgitimBolumId)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<int>>("OnlineEgitim/GetOnlineEgitimBolumTamamlananGecenSure/" + OnlinEgitimBolumId.ToString());
        }

        public async Task<ServiceResponse<List<OnlineEgitimSonucDTO>>> GetOnlineEgitimSonucList(int OnlineEgitimOgrenciId)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<OnlineEgitimSonucDTO>>>("OnlineEgitim/GetOnlineEgitimSonucList/" + OnlineEgitimOgrenciId.ToString());
        }

        public async Task<ServiceResponse<List<OnlineEgitimTestSonucDTO>>> GetOnlineEgitimTestSonuc(OnlineEgitimTestSonucFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("OnlineEgitim/GetOnlineEgitimTestSonuc", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<OnlineEgitimTestSonucDTO>>>();
        }
    }
}