using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IOnlineEgitimService
    {
        public Task<ServiceResponse<OnlineEgitimDTO>> GetOnlineEgitim(int Id);
        public Task<ServiceResponse<List<OnlineEgitimDTO>>> GetOnlineEgitimList(OnlineEgitimFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateOnlineEgitim(OnlineEgitimDTO dto);
        public Task<ServiceResponse<string>> DeleteOnlineEgitim(List<OnlineEgitimDTO> dtos);
        public Task<ServiceResponse<string>> InsertOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos);
        public Task<ServiceResponse<string>> DeleteOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos);
        public Task<ServiceResponse<List<OnlineEgitimTreeDTO>>> GetOnlineEgitimTreeList(int OnlineEgitimOgrenciId);
        public Task<ServiceResponse<string>> InsertUpdateOnlineEgitimBolumTamamlanan(OnlineEgitimBolumTamamlananDTO dto);
        public Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananGecenSure(OnlineEgitimTreeDTO dto);
        public Task<ServiceResponse<int>> GetOnlineEgitimBolumTamamlananGecenSure(int OnlinEgitimBolumId);
        public Task<ServiceResponse<List<OnlineEgitimSonucDTO>>> GetOnlineEgitimSonucList(int OnlineEgitimOgrenciId);
        public Task<ServiceResponse<List<OnlineEgitimTestSonucDTO>>> GetOnlineEgitimTestSonuc(OnlineEgitimTestSonucFilterDTO filterDTO);
    }
}