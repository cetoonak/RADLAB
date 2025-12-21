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
    public interface IIndirimKoduManager
    {
        public Task<ServiceResponse<List<IndirimKoduDTO>>> GetIndirimKoduList(IndirimKoduFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateIndirimKodu(IndirimKoduDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteIndirimKodu(List<IndirimKoduDTO> dtos, int KisiId);
    }
}