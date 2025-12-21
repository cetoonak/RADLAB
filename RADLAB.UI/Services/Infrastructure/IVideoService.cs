using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;

namespace RADLAB.UI.Services.Infrastructure
{
    public interface IVideoService
    {
        public Task<ServiceResponse<VideoDTO>> GetVideo(int Id);
        public Task<ServiceResponse<List<VideoDTO>>> GetVideoList(VideoFilterDTO filterDTO);
        public Task<ServiceResponse<string>> InsertOrUpdateVideo(VideoDTO dto);
        public Task<ServiceResponse<string>> DeleteVideo(List<VideoDTO> dtos);
    }
}