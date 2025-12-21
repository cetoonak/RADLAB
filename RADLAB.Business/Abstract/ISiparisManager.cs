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
    public interface ISiparisManager
    {
        public Task<ServiceResponse<List<SiparisDTO>>> GetSiparisList(SiparisFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<List<SiparisCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertSiparis(SiparisDTO dto, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateSiparis(SiparisDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteSiparis(List<SiparisDTO> dtos, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateSiparisCihaz(SiparisCihazDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteSiparisCihaz(List<SiparisCihazDTO> dtos, int KisiId);
        public Task<ServiceResponse<List<SiparisCihazHareketDTO>>> GetSiparisCihazHareket(int SiparisCihazId, int KisiId);
        public Task<ServiceResponse<string>> InsertSiparisCihazHareket(SiparisCihazHareketDTO dto, int KisiId);
    }
}