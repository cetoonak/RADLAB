using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class IlIlceService : IIlIlceService
    {
        private readonly HttpClient httpClient;

        public IlIlceService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<IlIlceDTO> GetIlIlce(int Id)
        {
            return await httpClient.GetFromJsonAsync<IlIlceDTO>("IlIlce/GetIlIlce/" + Id.ToString());
        }

        public async Task<List<IlIlceDTO>> GetIlIlceList(string Acilanlar)
        {
            return await httpClient.GetFromJsonAsync<List<IlIlceDTO>>("IlIlce/GetIlIlceList/" + Acilanlar);
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateIlIlce(IlIlceDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("IlIlce/InsertOrUpdateIlIlce", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteIlIlce(List<IlIlceDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("IlIlce/DeleteIlIlce", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}