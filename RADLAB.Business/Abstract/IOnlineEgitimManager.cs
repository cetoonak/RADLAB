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
    public interface IOnlineEgitimManager
    {
        public Task<ServiceResponse<OnlineEgitimDTO>> GetOnlineEgitim(int Id, int KisiId);
        public Task<ServiceResponse<List<OnlineEgitimDTO>>> GetOnlineEgitimList(OnlineEgitimFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateOnlineEgitim(OnlineEgitimDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteOnlineEgitim(List<OnlineEgitimDTO> dtos, int KisiId);
        public Task<ServiceResponse<string>> InsertOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos, int KisiId);
        public Task<ServiceResponse<string>> DeleteOnlineEgitimOgrenci(List<OnlineEgitimKisiDTO> dtos, int KisiId);
        public Task<ServiceResponse<List<OnlineEgitimTreeDTO>>> GetOnlineEgitimTreeList(int OnlineEgitimOgrenciId, int KisiId);
        public Task<ServiceResponse<string>> InsertUpdateOnlineEgitimBolumTamamlanan(OnlineEgitimBolumTamamlananDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananGecenSure(OnlineEgitimTreeDTO dto, int KisiId);
        public Task<ServiceResponse<int>> GetOnlineEgitimBolumTamamlananGecenSure(int OnlineEgitimBolumId, int KisiId);
        public Task<ServiceResponse<List<OnlineEgitimSonucDTO>>> GetOnlineEgitimSonucList(int OnlineEgitimOgrenciId, int KisiId);
        public Task<ServiceResponse<List<OnlineEgitimTestSonucDTO>>> GetOnlineEgitimTestSonuc(OnlineEgitimTestSonucFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> UpdateOnlineEgitimBolumTamamlananVideoTime(OnlineEgitimBolumTamamlananDTO dto, int KisiId);
    }
}