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
    public class IndirimKoduManager : IIndirimKoduManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public IndirimKoduManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<List<IndirimKoduDTO>>> GetIndirimKoduList(IndirimKoduFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<IndirimKoduDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectIndirimKodu
									@IndirimKodu,
									@AdSoyadUnvan,
									@BasvuruTakipNo,
									@Id,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<IndirimKoduDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<string>> InsertOrUpdateIndirimKodu(IndirimKoduDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO IndirimKodu
                                        (IndirimKodu,
                                        GSM,
		                                Tutar)
                                    VALUES
                                        (@IndirimKodu,
		                                @GSM,
		                                @Tutar)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE IndirimKodu SET
										IndirimKodu = @IndirimKodu,
                                        GSM = @GSM,
		                                Tutar = @Tutar
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

        public async Task<ServiceResponse<string>> DeleteIndirimKodu(List<IndirimKoduDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM IndirimKodu WHERE Id = {dto.Id}");
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