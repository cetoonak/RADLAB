using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ITanimSayiliManager
    {
        public Task<TanimSayiliDTO> GetTanimSayili(string Tanim, int Id);
        public Task<List<TanimSayiliDTO>> GetTanimSayiliList(string Tanim);
        public Task<ServiceResponse<string>> InsertOrUpdateTanimSayili(string Tanim, TanimSayiliDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteTanimSayili(string Tanim, List<TanimSayiliDTO> dtos, int KisiId);
    }
}