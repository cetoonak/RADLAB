using DocumentFormat.OpenXml.Office2010.Excel;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using RADLAB.UI.Services.Infrastructure;

namespace RADLAB.UI.Services.Services
{
    public class VideoService : IVideoService
    {
        private readonly HttpClient httpClient;

        public VideoService(HttpClient _httpClient)
        {
            httpClient = _httpClient;
        }

        public async Task<ServiceResponse<VideoDTO>> GetVideo(int Id)
        {
            return await httpClient.GetFromJsonAsync<ServiceResponse<VideoDTO>>("Video/GetVideo/" + Id.ToString());
        }

        public async Task<ServiceResponse<List<VideoDTO>>> GetVideoList(VideoFilterDTO filterDTO)
        {
            var result = await httpClient.PostAsJsonAsync("Video/GetVideoList", filterDTO);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<List<VideoDTO>>>();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateVideo(VideoDTO dto)
        {
            var result = await httpClient.PostAsJsonAsync("Video/InsertOrUpdateVideo", dto);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }

        public async Task<ServiceResponse<string>> DeleteVideo(List<VideoDTO> dtos)
        {
            var result = await httpClient.PostAsJsonAsync("Video/DeleteVideo", dtos);

            return await result.Content.ReadFromJsonAsync<ServiceResponse<string>>();
        }
    }
}