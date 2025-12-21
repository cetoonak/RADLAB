using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface IIlIlceManager
    {
        public Task<IlIlceDTO> GetIlIlce(int Id);
        public Task<List<IlIlceDTO>> GetIlIlceList(string Acilanlar);
        public Task<ServiceResponse<string>> InsertOrUpdateIlIlce(IlIlceDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteIlIlce(List<IlIlceDTO> dtos, int KisiId);
    }
}