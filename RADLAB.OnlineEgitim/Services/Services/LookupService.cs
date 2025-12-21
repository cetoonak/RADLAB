using RADLAB.Model.DTO;
using RADLAB.OnlineEgitim.Services.Infrastructure;
using System.Net.Http;

namespace RADLAB.OnlineEgitim.Services.Services
{
    public class LookupService : ILookupService
    {
        private readonly HttpClient httpClient;

        public LookupService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<List<LookupBasicDTO>> GetLookupBasic(string TableAndFieldName, string OrderFieldName = "")
        {
            if (string.IsNullOrEmpty(OrderFieldName))
            {
                OrderFieldName = TableAndFieldName;
            }

            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetLookupBasic/" + TableAndFieldName + "/" + OrderFieldName);
        }

        public async Task<List<LookupBasicDTO>> GetLookupBasicWithKod(string TableAndFieldName, string OrderFieldName = "")
        {
            if (string.IsNullOrEmpty(OrderFieldName))
            {
                OrderFieldName = TableAndFieldName;
            }

            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetLookupBasicWithKod/" + TableAndFieldName + "/" + OrderFieldName);
        }

        public async Task<List<LookupBasicDTO>> GetLookupFromMasterDetail(string TableAndFieldName, int ParentId)
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetLookupFromMasterDetail/" + TableAndFieldName + "/" + ParentId);
        }

        public async Task<List<LookupBasicDTO>> GetCihaz()
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetCihaz");
        }

        public async Task<List<LookupBasicDTO>> GetEgitimYili()
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetEgitimYili");
        }

        public async Task<List<LookupBasicDTO>> GetEgitimYapilanIl()
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetEgitimYapilanIl");
        }

        public async Task<List<LookupBasicDTO>> GetKullanici()
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetKullanici");
        }

        public async Task<List<LookupBasicDTO>> GetLookupDistinct(string TabledName, string FieldName)
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>($"Lookup/GetLookupDistinct/{TabledName}/{FieldName}");
        }

        public async Task<List<LookupBasicDTO>> GetTestVideo()
        {
            return await httpClient.GetFromJsonAsync<List<LookupBasicDTO>>("Lookup/GetTestVideo");
        }
    }
}