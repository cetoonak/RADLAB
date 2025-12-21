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
    public interface IAyarManager
    {
        public Task<AyarDTO> GetAyar();
        public Task<AyarDTO> GetAyarForOlcum();
        public Task<AyarKargoDTO> GetAyarForKargo(KargoAyarFilterDTO filterDTO);
        public Task<string> GetMetin(string field);
        public Task<ServiceResponse<string>> InsertOrUpdateAyar(AyarDTO dto, int KisiId);
    }
}