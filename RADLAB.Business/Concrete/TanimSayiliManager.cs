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
    public class TanimSayiliManager : ITanimSayiliManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public TanimSayiliManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<TanimSayiliDTO> GetTanimSayili(string Tanim, int Id)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<TanimSayiliDTO>($"SELECT Id, Sayi, {Tanim} AS Tanim FROM {Tanim} WHERE Id = {Id}");
        }

        public async Task<List<TanimSayiliDTO>> GetTanimSayiliList(string Tanim)
        {
            using var connection = new SqlConnection(CS);

            var result = await connection.QueryAsync<TanimSayiliDTO>($"SELECT Id, Sayi, {Tanim} AS Tanim FROM {Tanim} ORDER BY {Tanim}");

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateTanimSayili(string Tanim, TanimSayiliDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"IF @Id = 0
                                BEGIN
                                    INSERT INTO {Tanim}
                                        ({Tanim},
                                        Sayi)
                                    VALUES
                                        (@Tanim,
                                        @Sayi)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE {Tanim} SET
                                        {Tanim} = @Tanim,
                                        Sayi = @Sayi
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

        public async Task<ServiceResponse<string>> DeleteTanimSayili(string Tanim, List<TanimSayiliDTO> dtos, int KisiId)
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