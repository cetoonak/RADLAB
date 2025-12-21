using Dapper;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class DosyaManager : IDosyaManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public DosyaManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<DosyaDTO>> GetDosya(int Id, int KisiId)
        {
            var R = new ServiceResponse<DosyaDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                R.Value = await connection.QuerySingleOrDefaultAsync<DosyaDTO>("SELECT * FROM Dosya WHERE Id = @Id", new { Id = Id });
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

        public async Task<ServiceResponse<List<DosyaDTO>>> GetDosyaList(int KisiId)
        {
            var R = new ServiceResponse<List<DosyaDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<DosyaDTO>("SELECT Id, GorunenAd, DosyaAdi, Tip, Boyut / 1024 AS Boyut FROM Dosya");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, "", 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateDosya(DosyaDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Dosya
                                        (GorunenAd,
                                        DosyaAdi,
		                                Tip,
                                        Boyut,
                                        Dosya)
                                    VALUES
                                        (@GorunenAd,
                                        @DosyaAdi,
		                                @Tip,
                                        @Boyut,
                                        @Dosya)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Dosya SET
										GorunenAd = @GorunenAd
                                    WHERE
                                        Id = @Id

                                    SELECT @Id
                                END";

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

        public async Task<ServiceResponse<string>> DeleteDosya(List<DosyaDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Dosya WHERE Id = {dto.Id}");
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