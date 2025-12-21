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
    public interface ISoruManager
    {
        public Task<ServiceResponse<List<SoruDTO>>> GetSoruList(SoruFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateSoru(SoruDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteSoru(List<SoruDTO> dtos, int KisiId);
    }
}