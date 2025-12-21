using RADLAB.Model.DTO;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface ILookupService
    {
        public Task<List<LookupBasicDTO>> GetLookupBasic(string TableAndFieldName, string OrderFieldName = "");
        public Task<List<LookupBasicDTO>> GetLookupBasicWithKod(string TableAndFieldName, string OrderFieldName = "");
        public Task<List<LookupBasicDTO>> GetLookupFromMasterDetail(string TableAndFieldName, int ParentId);
        public Task<List<LookupBasicDTO>> GetCihaz();
        public Task<List<LookupBasicDTO>> GetEgitimYili();
        public Task<List<LookupBasicDTO>> GetEgitimYapilanIl();
        public Task<List<LookupBasicDTO>> GetKullanici();
        public Task<List<LookupBasicDTO>> GetLookupDistinct(string TabledName, string FieldName);
        public Task<List<LookupBasicDTO>> GetTestVideo();
    }
}