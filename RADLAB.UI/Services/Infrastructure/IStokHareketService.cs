using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IStokHareketService
    {
        public Task<ServiceResponse<List<StokHareketDTO>>> GetStokHareketList(StokHareketFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateStokHareket(StokHareketDTO dto);
        public Task<ServiceResponse<string>> DeleteStokHareket(List<StokHareketDTO> dtos);
        public Task<ServiceResponse<string>> InsertOrUpdateStokHareketCihaz(StokHareketCihazDTO dto);
        public Task<ServiceResponse<string>> DeleteStokHareketCihaz(List<StokHareketCihazDTO> dtos);
        public Task<ServiceResponse<List<StokHareketleriDTO>>> GetStokHareketleriList(StokHareketleriFilterDTO filterDTO);
        public Task<ServiceResponse<List<StokMiktarlariDTO>>> GetStokMiktarlariList(StokMiktarlariFilterDTO filterDTO);
    }
}