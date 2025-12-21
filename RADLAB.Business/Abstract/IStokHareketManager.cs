using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IStokHareketManager
    {
        public Task<ServiceResponse<List<StokHareketDTO>>> GetStokHareketList(StokHareketFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateStokHareket(StokHareketDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteStokHareket(List<StokHareketDTO> dtos, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateStokHareketCihaz(StokHareketCihazDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteStokHareketCihaz(List<StokHareketCihazDTO> dtos, int KisiId);
        public Task<ServiceResponse<List<StokHareketleriDTO>>> GetStokHareketleriList(StokHareketleriFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<List<StokMiktarlariDTO>>> GetStokMiktarlariList(StokMiktarlariFilterDTO filterDTO, int KisiId);
    }
}