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
    public class DashboardManager : IDashboardManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public DashboardManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<DashboardSayiDTO>> GetDashboardSayi(DashboardFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<DashboardSayiDTO>();

            try
            {
                filterDTO.KisiId = KisiId;

                using var connection = new SqlConnection(CS);

                var result = await connection.QuerySingleOrDefaultAsync<DashboardSayiDTO>($"SPSelectDashboard 'Sayilar', @Yil, @KisiId", filterDTO);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, new object(), 0);

                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<DashboardGrafikDTO>>> GetDashboardGrafik(DashboardFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<DashboardGrafikDTO>>();

            try
            {
                filterDTO.KisiId = KisiId;

                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<DashboardGrafikDTO>($"SPSelectDashboard @Tip, @Yil, @KisiId", filterDTO);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, new object(), 0);

                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<List<DuyuruDTO>>> GetDashboardDuyuru(int KisiId)
        {
            var R = new ServiceResponse<List<DuyuruDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  DECLARE @KisiId INT = {KisiId}

                                DECLARE @KisiTip INT

                                SELECT
	                                @KisiTip = Tip
                                FROM
	                                Kisi
                                WHERE
	                                Id = @KisiId

                                SELECT
	                                Id,
	                                Tarih,
	                                Konu,
                                    Icerik
                                FROM
	                                Duyuru
                                WHERE
	                                Aktif = 1
	                                AND CASE @KisiTip
			                                WHEN 1 THEN SistemKullanicilarinaGoster
			                                WHEN 2 THEN WebSitesindeGoster
		                                END = 1
                                ORDER BY
	                                Tarih DESC";

                var result = await connection.QueryAsync<DuyuruDTO>(Q);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, new object(), 0);

                R.Message = logReturn;
            }

            return R;
        }
    }
}