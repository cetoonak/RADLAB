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
    public class DuyuruManager : IDuyuruManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public DuyuruManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<DuyuruDTO> GetDuyuru(int Id)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<DuyuruDTO>($"SELECT * FROM Duyuru WHERE Id = {Id}");
        }

        public async Task<ServiceResponse<List<DuyuruDTO>>> GetDuyuruList(DuyuruFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<DuyuruDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT
	                                *
                                FROM
	                                Duyuru
                                WHERE
	                                Konu LIKE '%' + @Konu + '%'
	                                AND (SistemKullanicilarinaGoster = @SistemKullanicilarinaGoster OR @SistemKullanicilarinaGoster = -1)
	                                AND (WebSitesindeGoster = @WebSitesindeGoster OR @WebSitesindeGoster = -1)
	                                AND (Aktif = @Aktif OR @Aktif = -1)
                                ORDER BY
                                    Aktif DESC,
	                                Tarih DESC";

                var result = await connection.QueryAsync<DuyuruDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<string>> InsertOrUpdateDuyuru(DuyuruDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Duyuru
                                        (Tarih,
                                        Konu,
                                        Icerik,
                                        SistemKullanicilarinaGoster,
                                        WebSitesindeGoster,
                                        Aktif)
                                    VALUES
                                        (@Tarih,
                                        @Konu,
                                        @Icerik,
                                        @SistemKullanicilarinaGoster,
                                        @WebSitesindeGoster,
                                        @Aktif)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Duyuru SET
                                        Tarih = @Tarih,
                                        Konu = @Konu,
                                        Icerik = @Icerik,
                                        SistemKullanicilarinaGoster = @SistemKullanicilarinaGoster,
                                        WebSitesindeGoster = @WebSitesindeGoster,
                                        Aktif = @Aktif
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

        public async Task<ServiceResponse<string>> DeleteDuyuru(List<DuyuruDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Duyuru WHERE Id = {dto.Id}");
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