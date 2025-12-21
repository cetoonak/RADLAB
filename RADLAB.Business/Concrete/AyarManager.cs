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
    public class AyarManager : IAyarManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public AyarManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<AyarDTO> GetAyar()
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<AyarDTO>("SELECT TOP 1 * FROM Ayar");
        }

        public async Task<AyarDTO> GetAyarForOlcum()
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT TOP 1
                                ReferansTarihi,
                                Sicaklik1,
                                Sicaklik2,
                                Basinc1,
                                Basinc2,
                                Nem1,
                                Nem2,
	                            DDozCarpimDozHizi1,
	                            DDozCarpimDozHizi2,
	                            DDozCarpimDozHizi3,
	                            DDozCarpimDozHizi4,
	                            DDozCarpimDozHizi5,
	                            DDozCarpimToplamDoz,
	                            DDozCikarilanDozHizi1,
	                            DDozCikarilanDozHizi2,
	                            DDozCikarilanDozHizi3,
	                            DDozCikarilanDozHizi4,
	                            DDozCikarilanDozHizi5,
	                            DDozCikarilanToplamDoz
                            FROM
                                Ayar";

            return await connection.QuerySingleOrDefaultAsync<AyarDTO>(Q);
        }

        public async Task<AyarKargoDTO> GetAyarForKargo(KargoAyarFilterDTO filterDTO)
        {
            using var connection = new SqlConnection(CS);

            string Q = $@"  SELECT TOP 1
	                            Ayar.KargoFirmasiId,
	                            KargoFirmasi.KargoFirmasi,
	                            CASE
		                            WHEN Ayar.KargoBedavaLimitliDesili = 1 THEN 0
		                            WHEN Ayar.KargoBedavaLimitliDesili = 2 AND @SiparisToplami >= Ayar.KargoBedavaLimiti THEN 0
		                            WHEN Ayar.KargoBedavaLimitliDesili = 3 AND @Desi >= Ayar.KargoDesi5 THEN Ayar.KargoDesiUcret5
		                            WHEN Ayar.KargoBedavaLimitliDesili = 3 AND @Desi >= Ayar.KargoDesi4 THEN Ayar.KargoDesiUcret4
		                            WHEN Ayar.KargoBedavaLimitliDesili = 3 AND @Desi >= Ayar.KargoDesi3 THEN Ayar.KargoDesiUcret3
		                            WHEN Ayar.KargoBedavaLimitliDesili = 3 AND @Desi >= Ayar.KargoDesi2 THEN Ayar.KargoDesiUcret2
		                            WHEN Ayar.KargoBedavaLimitliDesili = 3 AND @Desi >= Ayar.KargoDesi1 THEN Ayar.KargoDesiUcret1
		                            ELSE 0
	                            END AS KargoUcreti
                            FROM
	                            Ayar
	                            LEFT JOIN KargoFirmasi ON Ayar.KargoFirmasiId = KargoFirmasi.Id";

            return await connection.QuerySingleOrDefaultAsync<AyarKargoDTO>(Q, filterDTO);
        }

        public async Task<string> GetMetin(string field)
        {
            using var connection = new SqlConnection(CS);

            return await connection.QuerySingleOrDefaultAsync<string>($"SELECT TOP 1 {field} FROM Ayar");
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateAyar(AyarDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

				string Q = $@"SPInsertUpdateAyar
									@KurumAdi,
									@Adres,
									@Telefon,
									@Faks,
									@EMail,
									@WebAdresi,
									@VergiDairesi,
									@VergiNo,
									@Banka,
									@IBAN,
									@EMailSunucusu,
									@EMailGonderenAdres,
									@EMailGonderenSifre,
									@EMailPort,
									@EMailSSL,
	                                @KisiIdLaboratuvarSorumlusu,
	                                @KisiIdKurumSorumlusu,
                                    @KargoFirmasiId,
                                    @KargoBedavaLimitliDesili,
                                    @KargoBedavaLimiti,
									@KargoDesi1,
									@KargoDesi2,
									@KargoDesi3,
									@KargoDesi4,
									@KargoDesi5,
									@KargoDesiUcret1,
									@KargoDesiUcret2,
									@KargoDesiUcret3,
									@KargoDesiUcret4,
									@KargoDesiUcret5,
                                    @DamgaVergisiOrani,
									@SozlesmePuluOrani,
                                    @ReferansTarihi,
                                    @Sicaklik1,
                                    @Sicaklik2,
                                    @Basinc1,
                                    @Basinc2,
                                    @Nem1,
                                    @Nem2,
	                                @DDozCarpimDozHizi1,
	                                @DDozCarpimDozHizi2,
	                                @DDozCarpimDozHizi3,
	                                @DDozCarpimDozHizi4,
	                                @DDozCarpimDozHizi5,
	                                @DDozCarpimToplamDoz,
	                                @DDozCikarilanDozHizi1,
	                                @DDozCikarilanDozHizi2,
	                                @DDozCikarilanDozHizi3,
	                                @DDozCikarilanDozHizi4,
	                                @DDozCikarilanDozHizi5,
	                                @DDozCikarilanToplamDoz,
									@ResimAntetUst,
									@ResimAntetAlt,
									@ResimLogo,
									@ResimTurkak,
                                    @RadyoaktifKaynakYuklemeMetni,
                                    @GizlilikSozlesmesiMetni,
                                    @OnBilgilendirmeFormuMetni,
                                    @MesafeliSatisSozlesmesiMetni,
                                    @IadeSozlesmesiMetni,
                                    @MailSablonu";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                R.Value = result;
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