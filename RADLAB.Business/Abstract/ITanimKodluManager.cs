using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ITanimKodluManager
    {
        public Task<TanimKodluDTO> GetTanimKodlu(string Tanim, int Id);
        public Task<List<TanimKodluDTO>> GetTanimKodluList(string Tanim);
        public Task<ServiceResponse<string>> InsertOrUpdateTanimKodlu(string Tanim, TanimKodluDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteTanimKodlu(string Tanim, List<TanimKodluDTO> dtos, int KisiId);
    }
}