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
    public class TestManager : ITestManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public TestManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<TestDTO>> GetTest(int Id, int KisiId)
        {
            var R = new ServiceResponse<TestDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                var data = await connection.QuerySingleOrDefaultAsync<TestDTO>($"SELECT * FROM Test WHERE Id = {Id}");

                string QSoruList = $@"  SELECT
	                                        Soru.Id,
	                                        Soru.Soru,
                                            Soru.Sira,
                                            Soru.Zorluk
                                        FROM
	                                        TestSoru
	                                        LEFT JOIN Soru ON TestSoru.SoruId = Soru.Id
                                        WHERE
                                            TestSoru.TestId = {Id.ToString()}
                                        ORDER BY
                                            Soru.Sira";

                var resultSoruList = await connection.QueryAsync<SoruDTO>(QSoruList);

                data.SoruList = resultSoruList.ToList();

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

        public async Task<ServiceResponse<List<TestDTO>>> GetTestList(TestFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<TestDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectTest
	                                @Test,
	                                @Sure,
	                                @GecmeOrani,
	                                @Id,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<TestDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<string>> InsertOrUpdateTest(TestDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                var dtoParam = JsonConvert.DeserializeObject<TestParamDTO>(JsonConvert.SerializeObject(dto));
                dtoParam.SoruIdler = string.Join("_", dtoParam.SoruList.Select(x => x.Id).ToList());

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Test
                                        (Test,
                                        Aciklama,
                                        Sure,
                                        GecmeOrani)
                                    VALUES
                                        (@Test,
                                        @Aciklama,
                                        @Sure,
                                        @GecmeOrani)

                                    SELECT @Id = @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Test SET
                                        Test = @Test,
                                        Aciklama = @Aciklama,
                                        Sure = @Sure,
                                        GecmeOrani = @GecmeOrani
                                    WHERE
                                        Id = @Id

                                    DELETE FROM TestSoru WHERE TestId = @Id
                                END

                                IF @SoruIdler != ''
                                BEGIN
                                    INSERT INTO TestSoru
                                        (TestId, SoruId)
                                    SELECT
                                        @Id AS TestId,
                                        Id AS SoruId
                                    FROM
                                        dbo.FIdList(@SoruIdler)
                                END

                                SELECT @Id";

                var result = await connection.ExecuteScalarAsync<string>(Q, dtoParam);

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

        public async Task<ServiceResponse<string>> DeleteTest(List<TestDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM TestSoru WHERE TestId = {dto.Id} DELETE FROM Test WHERE Id = {dto.Id}");
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