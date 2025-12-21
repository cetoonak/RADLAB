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
    public interface IMesajManager
    {
        public Task<ServiceResponse<MesajDTO>> GetMesaj(int GelenGiden, int Id, int KisiId);
        public Task<ServiceResponse<int>> GetOkunmamisMesajSayisi(int KisiId);
        public Task<ServiceResponse<List<MesajDTO>>> GetOkunmamisMesajlar(int KisiId);
        public Task<ServiceResponse<List<MesajDTO>>> GetMesajKutusu(MesajKutusuFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<MesajGrubuMasterDTO>> GetMesajGrubuMaster(int Id, int KisiId);
        public Task<ServiceResponse<List<MesajGrubuMasterDTO>>> GetMesajGrubuMasterList(int KisiId);
        public Task<ServiceResponse<List<MesajKisiDTO>>> GetMesajKisiList(string SearchText, int KisiId);
        public Task<ServiceResponse<string>> InsertMesaj(MesajYazDTO dto, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateMesajGrubu(MesajGrubuMasterDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateMesajGonderilenKisiOkundu(MesajDTO dto, int KisiId);
        public Task<ServiceResponse<string>> UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl(MesajDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteMesajGrubuMaster(MesajGrubuMasterDTO dto, int KisiId);
    }
}