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
    public interface IVideoManager
    {
        public Task<ServiceResponse<VideoDTO>> GetVideo(int Id, int KisiId);
        public Task<ServiceResponse<List<VideoDTO>>> GetVideoList(VideoFilterDTO filterDTO, int KisiId);
        public Task<ServiceResponse<string>> InsertOrUpdateVideo(VideoDTO dto, int KisiId);
        public Task<ServiceResponse<string>> DeleteVideo(List<VideoDTO> dtos, int KisiId);
    }
}