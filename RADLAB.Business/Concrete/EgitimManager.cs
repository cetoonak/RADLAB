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
    public class EgitimManager : IEgitimManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public EgitimManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<EgitimDTO> GetEgitim(int Id)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<EgitimDTO>($"SELECT * FROM Egitim WHERE Id = {Id}");
        }

        public async Task<List<EgitimDTO>> GetEgitimList()
        {
            using var connection = new SqlConnection(CS);

            var result = await connection.QueryAsync<EgitimDTO>("SELECT * FROM Egitim ORDER BY Sira");

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateEgitim(EgitimDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"IF @Id = 0
                                BEGIN
                                    INSERT INTO Egitim
                                        (Sira,
                                        Egitim,
                                        Baslik,
                                        Kisaltma,
                                        NormalGunluk,
                                        Amac,
                                        Sart)
                                    VALUES
                                        (@Sira,
                                        @Egitim,
                                        @Baslik,
                                        @Kisaltma,
                                        @NormalGunluk,
                                        @Amac,
                                        @Sart)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Egitim SET
                                        Sira = @Sira,
                                        Egitim = @Egitim,
                                        Baslik = @Baslik,
                                        Kisaltma = @Kisaltma,
                                        NormalGunluk = @NormalGunluk,
                                        Amac = @Amac,
                                        Sart = @Sart
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

        public async Task<ServiceResponse<string>> DeleteEgitim(List<EgitimDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Egitim WHERE Id = {dto.Id}");
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