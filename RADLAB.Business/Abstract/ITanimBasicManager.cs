using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ITanimBasicManager
    {
        public Task<TanimBasicDTO> GetTanimBasic(string Tanim, int Id);
        public Task<List<TanimBasicDTO>> GetTanimBasicList(string Tanim);
        public Task<ServiceResponse<string>> InsertOrUpdateTanimBasic(string Tanim, TanimBasicDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteTanimBasic(string Tanim, List<TanimBasicDTO> dtos, int KisiId);
    }
}