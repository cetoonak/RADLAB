using Dapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ParamDTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class VideoManager : IVideoManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public VideoManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<VideoDTO>> GetVideo(int Id, int KisiId)
        {
            var R = new ServiceResponse<VideoDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT
			                        Video.*,
			                        Klasor.Klasor
		                        FROM
			                        Video
			                        LEFT JOIN Klasor ON Video.KlasorId = Klasor.Id
                                WHERE
                                    Video.Id = {Id.ToString()}";

                var data = await connection.QuerySingleOrDefaultAsync<VideoDTO>(Q);

                R.Value = data;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, Id, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<VideoDTO>>> GetVideoList(VideoFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<VideoDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectVideo
	                                @DosyaAdi,
	                                @KlasorId,
	                                @Id,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<VideoDTO>(Q, filterDTO);

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(Q, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, filterDTO, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateVideo(VideoDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Video
                                        (KlasorId,
                                        DosyaAdi,
                                        Uzanti,
                                        Dosya)
                                    VALUES
                                        (@KlasorId,
                                        @DosyaAdi,
                                        @Uzanti,
                                        @Dosya)

                                    SELECT @Id = @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Video SET
                                        KlasorId = @KlasorId,
                                        DosyaAdi = @DosyaAdi,
                                        Uzanti = @Uzanti,
                                        Dosya = @Dosya
                                    WHERE
                                        Id = @Id
                                END

                                SELECT @Id";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> DeleteVideo(List<VideoDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Video WHERE Id = {dto.Id}");
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dtos, dtos[0].Id);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }
    }
}