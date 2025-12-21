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
    public interface IKursManager
    {
        public Task<ServiceResponse<List<KursDTO>>> GetKursList(KursFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateKurs(KursDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteKurs(List<KursDTO> dtos, int KisiId);
        public Task<ServiceResponse<List<KursYayinDTO>>> GetKursYayinList(int Id, int KisiId);
        public Task<ServiceResponse<List<KursiyerTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<KursiyerDTO>> GetKursiyer(int Id, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateKursiyer(KursiyerDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteKursiyer(List<KursiyerDTO> dtos, int KisiId);
    }
}