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
    public interface IDosyaManager
    {
        public Task<ServiceResponse<DosyaDTO>> GetDosya(int Id, int KisiId);
        public Task<ServiceResponse<List<DosyaDTO>>> GetDosyaList(int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateDosya(DosyaDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteDosya(List<DosyaDTO> dtos, int KisiId);
    }
}