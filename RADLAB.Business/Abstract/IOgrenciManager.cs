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
    public interface IOgrenciManager
    {
        public Task<OgrenciDTO> GetOgrenci(int Id);
        public Task<ServiceResponse<List<OgrenciDTO>>> GetOgrenciList(OgrenciFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateOgrenci(OgrenciDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateOgrenciAktif(List<OgrenciDTO> dtos, int KisiId);
        public Task<ServiceResponse<string>> DeleteOgrenci(List<OgrenciDTO> dtos, int KisiId);
    }
}