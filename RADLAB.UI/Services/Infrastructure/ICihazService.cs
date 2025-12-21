using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ICihazService
    {
        public Task<CihazDTO> GetCihaz(int Id);
        public Task<List<CihazDTO>> GetCihazList(string Acilanlar);
        public Task<List<CihazDTO>> GetCihazSatisList();
        public Task<ServiceResponse<string>> InsertOrUpdateCihaz(CihazDTO dto);
        public Task<ServiceResponse<string>> DeleteCihaz(List<CihazDTO> dtos);
    }
}