using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IMesajService
    {
        public Task<ServiceResponse<MesajDTO>> GetMesaj(int GelenGiden, int IdMesaj);
        public Task<ServiceResponse<int>> GetOkunmamisMesajSayisi();
        public Task<ServiceResponse<List<MesajDTO>>> GetOkunmamisMesajlar();
        public Task<ServiceResponse<List<MesajDTO>>> GetMesajKutusu(MesajKutusuFilterDTO filterDTO);
        public Task<ServiceResponse<MesajGrubuMasterDTO>> GetMesajGrubuMaster(int Id);
        public Task<ServiceResponse<List<MesajGrubuMasterDTO>>> GetMesajGrubuMasterList();
        public Task<ServiceResponse<List<MesajKisiDTO>>> GetMesajKisiList(string SearchText);
        public Task<ServiceResponse<string>> InsertMesaj(MesajYazDTO dto);
        public Task<ServiceResponse<string>> InsertOrUpdateMesajGrubu(MesajGrubuMasterDTO dto);
        public Task<ServiceResponse<string>> UpdateMesajGonderilenKisiOkundu(MesajDTO dto);
        public Task<ServiceResponse<string>> UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl(MesajDTO dto);
        public Task<ServiceResponse<string>> DeleteMesajGrubuMaster(MesajGrubuMasterDTO dto);
    }
}