using RADLAB.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Abstract
{
    public interface ILookupManager
    {
        public Task<List<LookupBasicDTO>> GetLookupBasic(string TableAndFieldName, string OrderFieldName);
        public Task<List<LookupBasicDTO>> GetLookupBasicWithKod(string TableAndFieldName, string OrderFieldName);
        public Task<List<LookupBasicDTO>> GetLookupFromMasterDetail(string TableAndFieldName, int ParentId);
        public Task<List<LookupBasicDTO>> GetCihaz();
        public Task<List<LookupBasicDTO>> GetEgitimYili();
        public Task<List<LookupBasicDTO>> GetEgitimYapilanIl();
        public Task<List<LookupBasicDTO>> GetKullanici();
        public Task<List<LookupBasicDTO>> GetLookupDistinct(string TabledName, string FieldName);
        public Task<List<LookupBasicDTO>> GetTestVideo();
    }
}