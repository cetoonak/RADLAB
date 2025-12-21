using Dapper;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System.Data.SqlClient;
using System.Reflection;

namespace RADLAB.Business.Concrete
{
    public class TanimBasicManager : ITanimBasicManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public TanimBasicManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<TanimBasicDTO> GetTanimBasic(string Tanim, int Id)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<TanimBasicDTO>($"SELECT Id, {Tanim} AS Tanim FROM {Tanim} WHERE Id = {Id}");
        }

        public async Task<List<TanimBasicDTO>> GetTanimBasicList(string Tanim)
        {
            using var connection = new SqlConnection(CS);

            var result = await connection.QueryAsync<TanimBasicDTO>($"SELECT Id, {Tanim} AS Tanim FROM {Tanim} ORDER BY {Tanim}");

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateTanimBasic(string Tanim, TanimBasicDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"IF @Id = 0
                                BEGIN
                                    INSERT INTO {Tanim}
                                        ({Tanim})
                                    VALUES
                                        (@Tanim)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE {Tanim} SET
                                        {Tanim} = @Tanim
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

        public async Task<ServiceResponse<string>> DeleteTanimBasic(string Tanim, List<TanimBasicDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM {Tanim} WHERE Id = {dto.Id}");
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
