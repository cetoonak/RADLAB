using Dapper;
using Microsoft.Extensions.Configuration;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using RADLAB.Model.DTO;
using RADLAB.Model.FilterDTO;
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
    public class OdemeManager : IOdemeManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public OdemeManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<List<OdemeDTO>>> GetOdeme(MusteriTakipFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<OdemeDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<OdemeDTO>("SPSelectOdeme @GSM, @Mail", filterDTO);

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

        public async Task<ServiceResponse<List<OdemeDTO>>> GetOdemeByVerifyEnrollmentRequestId(string VerifyEnrollmentRequestId, int KisiId)
        {
            var R = new ServiceResponse<List<OdemeDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<OdemeDTO>($"SPSelectOdemeByVerifyEnrollmentRequestId '{VerifyEnrollmentRequestId}'");

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, VerifyEnrollmentRequestId, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateOdeme(List<OdemeDTO> dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Tablo = 'Kalibrasyon'
                                BEGIN
	                                UPDATE KalibrasyonCihaz SET
		                                OdemeTarihi = GETDATE()
	                                WHERE
		                                KalibrasyonId = @TabloId
                                END ELSE
                                IF @Tablo = 'KalibrasyonCihazOdeme'
                                BEGIN
	                                UPDATE KalibrasyonCihazOdeme SET
		                                OdemeTarihi = GETDATE()
	                                WHERE
		                                Id = @TabloId
                                END ELSE
                                IF @Tablo = 'Kursiyer'
                                BEGIN
	                                UPDATE Kursiyer SET
		                                OdemeZamani = GETDATE()
	                                WHERE
		                                Id = @TabloId
                                END

                                SELECT @TabloId";

                foreach (var item in dto)
                {
                    await connection.ExecuteScalarAsync<string>(Q, item);
                }

                R.Value = "Ok";
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, 0);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateOdemeVerifyEnrollmentRequestId(List<OdemeDTO> dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Tablo = 'Kalibrasyon'
                                BEGIN
	                                UPDATE Kalibrasyon SET
		                                VerifyEnrollmentRequestId = @VerifyEnrollmentRequestId,
                                        CVV = @CVV
	                                WHERE
		                                Id = @TabloId
                                END ELSE
                                IF @Tablo = 'KalibrasyonCihazOdeme'
                                BEGIN
	                                UPDATE KalibrasyonCihazOdeme SET
		                                VerifyEnrollmentRequestId = @VerifyEnrollmentRequestId,
                                        CVV = @CVV
	                                WHERE
		                                Id = @TabloId
                                END ELSE
                                IF @Tablo = 'Kursiyer'
                                BEGIN
	                                UPDATE Kursiyer SET
		                                VerifyEnrollmentRequestId = @VerifyEnrollmentRequestId,
                                        CVV = @CVV
	                                WHERE
		                                Id = @TabloId
                                END

                                SELECT @TabloId";

                foreach (var item in dto)
                {
                    await connection.ExecuteScalarAsync<string>(Q, item);
                }

                R.Value = "Ok";
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var LogReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, 0);

            if (!R.Success) R.Message = LogReturn;

            return R;
        }
    }
}