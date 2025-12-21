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
    public interface IEgitimManager
    {
        public Task<EgitimDTO> GetEgitim(int Id);
        public Task<List<EgitimDTO>> GetEgitimList();
        public Task<ServiceResponse<string>> InsertOrUpdateEgitim(EgitimDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteEgitim(List<EgitimDTO> dtos, int KisiId);
    }
}