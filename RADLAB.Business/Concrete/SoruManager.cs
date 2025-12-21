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
    public class SoruManager : ISoruManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public SoruManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<List<SoruDTO>>> GetSoruList(SoruFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<SoruDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectSoru
	                                @Soru,
	                                @Etiket,
	                                @Zorluk,
	                                @Id,
									@PageNo,
									@PageSize,
									@DonusTipi";

                var result = await connection.QueryAsync<SoruDTO>(Q, filterDTO);

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

        public async Task<ServiceResponse<string>> InsertOrUpdateSoru(SoruDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Soru
                                        (Soru,
                                        DosyaAdi,
		                                Uzanti,
                                        Dosya,
                                        CevapA,
                                        CevapB,
                                        CevapC,
                                        CevapD,
                                        CevapE,
                                        Cevap,
                                        Sira,
                                        Zorluk,
                                        Etiket)
                                    VALUES
                                        (@Soru,
                                        @DosyaAdi,
		                                @Uzanti,
                                        @Dosya,
                                        @CevapA,
                                        @CevapB,
                                        @CevapC,
                                        @CevapD,
                                        @CevapE,
                                        @Cevap,
                                        @Sira,
                                        @Zorluk,
                                        @Etiket)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Soru SET
                                        Soru = @Soru,
                                        DosyaAdi = @DosyaAdi,
		                                Uzanti = @Uzanti,
                                        Dosya = @Dosya,
                                        CevapA = @CevapA,
                                        CevapB = @CevapB,
                                        CevapC = @CevapC,
                                        CevapD = @CevapD,
                                        CevapE = @CevapE,
                                        Cevap = @Cevap,
                                        Sira = @Sira,
                                        Zorluk = @Zorluk,
                                        Etiket = @Etiket
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

        public async Task<ServiceResponse<string>> DeleteSoru(List<SoruDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Soru WHERE Id = {dto.Id}");
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