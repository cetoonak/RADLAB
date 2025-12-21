using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IIlIlceService
    {
        public Task<IlIlceDTO> GetIlIlce(int Id);
        public Task<List<IlIlceDTO>> GetIlIlceList(string Acilanlar);
        public Task<ServiceResponse<string>> InsertOrUpdateIlIlce(IlIlceDTO dto);
        public Task<ServiceResponse<string>> DeleteIlIlce(List<IlIlceDTO> dtos);
    }
}