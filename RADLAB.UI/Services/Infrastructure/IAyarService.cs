using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IAyarService
    {
        public Task<AyarDTO> GetAyar();
        public Task<AyarDTO> GetAyarForOlcum();
        public Task<AyarKargoDTO> GetAyarForKargo(KargoAyarFilterDTO filterDTO);
        public Task<string> GetMetin(string field);
        public Task<ServiceResponse<string>> InsertOrUpdateAyar(AyarDTO dto);
    }
}