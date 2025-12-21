using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class MesajService : IMesajService
    {
        private readonly HttpClient httpClient;

        public MesajService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<MesajDTO>> GetMesaj(int GelenGiden, int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<MesajDTO>>($"Mesaj/GetMesaj/{GelenGiden}/{Id}");
        }

        public async Task<ServiceResponse<int>> GetOkunmamisMesajSayisi()
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<int>>("Mesaj/GetOkunmamisMesajSayisi");
        }

        public async Task<ServiceResponse<List<MesajDTO>>> GetOkunmamisMesajlar()
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<MesajDTO>>>($"Mesaj/GetOkunmamisMesajlar");
        }

        public async Task<ServiceResponse<List<MesajDTO>>> GetMesajKutusu(MesajKutusuFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Mesaj/GetMesajKutusu", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<MesajDTO>>>();
        }

        public async Task<ServiceResponse<MesajGrubuMasterDTO>> GetMesajGrubuMaster(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<MesajGrubuMasterDTO>>($"Mesaj/GetMesajGrubuMaster/{Id}");
        }

        public async Task<ServiceResponse<List<MesajGrubuMasterDTO>>> GetMesajGrubuMasterList()
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<MesajGrubuMasterDTO>>>("Mesaj/GetMesajGrubuMasterList");
        }

        public async Task<ServiceResponse<List<MesajKisiDTO>>> GetMesajKisiList(string SearchText)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<List<MesajKisiDTO>>>($"Mesaj/GetMesajKisiList/{SearchText}");
        }

        public async Task<ServiceResponse<string>> InsertMesaj(MesajYazDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Mesaj/InsertMesaj", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateMesajGrubu(MesajGrubuMasterDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Mesaj/InsertOrUpdateMesajGrubu", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateMesajGonderilenKisiOkundu(MesajDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Mesaj/UpdateMesajGonderilenKisiOkundu", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl(MesajDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Mesaj/UpdateMesajGonderilenKisiVeyaMesajSilVeyaGeriAl", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteMesajGrubuMaster(MesajGrubuMasterDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Mesaj/DeleteMesajGrubuMaster", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}