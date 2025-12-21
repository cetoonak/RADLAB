using Dapper;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.ResponseModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RADLAB.Business.Concrete
{
    public class IlIlceManager : IIlIlceManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public IlIlceManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<IlIlceDTO> GetIlIlce(int Id)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<IlIlceDTO>($"SELECT * FROM ViewIlIlce WHERE Id = {Id}");
        }

        public async Task<List<IlIlceDTO>> GetIlIlceList(string Acilanlar)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"SELECT
	                            *,
	                            CASE
		                            WHEN ChildCount = 0 THEN ''
		                            WHEN CONVERT(VARCHAR(50), Id) IN (SELECT Id FROM dbo.FIdList(@Acilanlar)) THEN 'bi bi-chevron-down'
		                            ELSE 'bi bi-chevron-right'
	                            END AS Icon
                            FROM
	                            (SELECT
	                                V.*,
		                            (SELECT
			                            COUNT(IlIlceIc.Id)
		                            FROM
			                            IlIlce IlIlceIc
		                            WHERE
			                            IlIlceIc.ParentId = V.Id) AS ChildCount
                                FROM
	                                ViewIlIlce V
	                            WHERE
		                            (V.Seviye = 0 OR
		                            CONVERT(VARCHAR(50), V.Id) IN (SELECT Id FROM dbo.FIdList(@Acilanlar)) OR
                                    CONVERT(VARCHAR(50), V.ParentId) IN (SELECT Id FROM dbo.FIdList(@Acilanlar)))) AS S
                            ORDER BY
	                            AdAcilimi";

            var result = await connection.QueryAsync<IlIlceDTO>(Q, new { Acilanlar = Acilanlar });

            return result.ToList();
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateIlIlce(IlIlceDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"IF @Id = 0
                                BEGIN
                                    INSERT INTO IlIlce
                                        (ParentId,
                                        Seviye,
                                        IlIlce,
                                        Kod,
                                        EgitimYapilanIl)
                                    VALUES
                                        ((CASE WHEN @ParentId = 0 THEN NULL ELSE @ParentId END),
                                        @Seviye,
                                        @IlIlce,
                                        @Kod,
                                        @EgitimYapilanIl)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE IlIlce SET
                                        IlIlce = @IlIlce,
                                        Kod = @Kod,
                                        EgitimYapilanIl = @EgitimYapilanIl
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

        public async Task<ServiceResponse<string>> DeleteIlIlce(List<IlIlceDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM IlIlce WHERE ParentId = {dto.Id} DELETE FROM IlIlce WHERE Id = {dto.Id}");
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