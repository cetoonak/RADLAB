using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using RADLAB.Model.DTO;
using RADLAB.Business.Abstract;
using RADLAB.Business.Utils;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RADLAB.Model.ParamDTO;
using RADLAB.Model.ResponseModels;
using System.Reflection;
using RADLAB.Model.FilterDTO;

namespace RADLAB.Business.Concrete
{
    public class KisiManager : IKisiManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public KisiManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<KisiDTO> GetKisi(int Id)
        {
            using var connection = new SqlConnection(CS);

            var Kisi = await connection.QuerySingleOrDefaultAsync<KisiDTO>($"SELECT * FROM Kisi WHERE Id = {Id}");

            return Kisi;
        }

        public async Task<ServiceResponse<string>> UpdateKisiProfil(KullaniciForLoginDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                await connection.ExecuteAsync($@"   UPDATE Kisi SET
                                                        TelefonCep = @TelefonCep,
                                                        EMail = @EMail
                                                    WHERE
                                                        Id = @Id", dto);
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, KisiId);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateKisiOnayAcikRiza(List<KisiOnayDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"UPDATE Kisi SET AcikRiza = {dto.AcikRiza} WHERE Id = {dto.Id}");
                }
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dtos, dtos[0].Id);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateKisiCookiePolitikasiniGordu(KullaniciForLoginDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                int value = dto.CookiePolitikasiniGordu ? 1 : 0;

                await connection.ExecuteAsync($"UPDATE Kisi SET CookiePolitikasiniGordu = {value} WHERE Id = {dto.Id}");
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateKisiAcikRiza(KullaniciForLoginDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                int value = dto.AcikRiza ? 1 : 0;

                await connection.ExecuteAsync($"UPDATE Kisi SET AcikRiza = {value} WHERE Id = {dto.Id}");
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = logReturn;

            return R;
        }

        public async Task<ServiceResponse<string>> UpdateKisiGizlilikOnaylandi(KullaniciForLoginDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                int value = dto.GizlilikOnaylandi ? 1 : 0;

                await connection.ExecuteAsync($"UPDATE Kisi SET GizlilikOnaylandi = {value} WHERE Id = {dto.Id}");
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, dto, dto.Id);

            if (!R.Success) R.Message = logReturn;

            return R;
        }
    }
}