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
    public interface IDuyuruManager
    {
        public Task<DuyuruDTO> GetDuyuru(int Id);
        public Task<ServiceResponse<List<DuyuruDTO>>> GetDuyuruList(DuyuruFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateDuyuru(DuyuruDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteDuyuru(List<DuyuruDTO> dtos, int KisiId);
    }
}