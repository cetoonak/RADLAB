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
    public class RaporManager : IRaporManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public RaporManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<string>> GetRaporBarkod(FRDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                int KullaniciTipi = await connection.ExecuteScalarAsync<Int32>($"SELECT Tip FROM Kisi WHERE Id = {KisiId}");

                int KisiIdByTCKimlikNo = 0;
                
                if (dto.ReportNameOrBase64 == "KisiselListe")
                {
                    KisiIdByTCKimlikNo = await connection.ExecuteScalarAsync<Int32>($"SELECT TOP 1 Id FROM Kisi WHERE TCKimlikNo = '{dto.TCKimlikNo}'");
                }

                string Q = $@"SPInsertSelectRaporBarkod
                                    @KurulusId,
                                    @BirimIdler,
                                    @Donem,
                                    @AyriAyri,
                                    {KisiIdByTCKimlikNo},
                                    @Donemler,
                                    {KullaniciTipi}";

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
    }
}