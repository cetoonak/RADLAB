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
    public class SiparisManager : ISiparisManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public SiparisManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<List<SiparisDTO>>> GetSiparisList(SiparisFilterDTO filterDTO, int KisiId)
        {
            try
            {
                string tarih1 = filterDTO.SiparisTarihiAraligi.Split('-')[0].Trim();
                string tarih2 = filterDTO.SiparisTarihiAraligi.Split('-')[1].Trim();

                int tarih1Gun = Convert.ToInt16(tarih1.Split('.')[0]);
                int tarih1Ay = Convert.ToInt16(tarih1.Split('.')[1]);
                int tarih1Yil = Convert.ToInt16(tarih1.Split('.')[2]);

                int tarih2Gun = Convert.ToInt16(tarih2.Split('.')[0]);
                int tarih2Ay = Convert.ToInt16(tarih2.Split('.')[1]);
                int tarih2Yil = Convert.ToInt16(tarih2.Split('.')[2]);

                filterDTO.SiparisTarihi1 = Convert.ToInt32((new DateTime(tarih1Yil, tarih1Ay, tarih1Gun)).ToOADate());
                filterDTO.SiparisTarihi2 = Convert.ToInt32((new DateTime(tarih2Yil, tarih2Ay, tarih2Gun)).ToOADate());
            }
            catch
            {
                filterDTO.SiparisTarihi1 = 0;
                filterDTO.SiparisTarihi2 = 0;
            }

            var R = new ServiceResponse<List<SiparisDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QSiparis = $@"SPSelectSiparis
	                                    @GSM,
										@Mail,
										@AdSoyadSiparis,
										@TCKimlikNoSiparis,
										@IlId,
										@SiparisTarihi1,
										@SiparisTarihi2,
										@SiparisDurumu,
										@Order,
										@Id,
                                        @VerifyEnrollmentRequestId,
										@PageNo,
										@PageSize,
										@DonusTipi";

                var resultSiparis = await connection.QueryAsync<SiparisDTO>(QSiparis, filterDTO);

                foreach (var siparis in resultSiparis)
                {
                    string QSiparisCihaz = $@"  SELECT
	                                                SiparisCihaz.*,
                                                    Model.ParentId AS MarkaId,
                                                    SiparisCihaz.CihazId AS ModelId,
	                                                Marka.Cihaz AS Marka,
	                                                Model.Cihaz AS Model,
	                                                ISNULL(SCH.SiparisCihazHareketTuru, 'Yeni Sipariş') AS Durum,
	                                                ISNULL(SCH.Badge, 'badge bg-primary') AS Badge,
	                                                ISNULL(SCH.IslemZamani, Siparis.SiparisZamani) AS SonIslemZamani
                                                FROM
	                                                SiparisCihaz
	                                                LEFT JOIN Siparis ON SiparisCihaz.SiparisId = Siparis.Id
	                                                LEFT JOIN Cihaz Model ON SiparisCihaz.CihazId = Model.Id
	                                                LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
	                                                OUTER APPLY (SELECT TOP 1
					                                                SiparisCihazHareket.IslemZamani,
					                                                SiparisCihazHareketTuru.SiparisCihazHareketTuru,
					                                                SiparisCihazHareketTuru.Badge
				                                                FROM
					                                                SiparisCihazHareket
					                                                LEFT JOIN SiparisCihazHareketTuru ON SiparisCihazHareket.SiparisCihazHareketTuruId = SiparisCihazHareketTuru.Id
				                                                WHERE
					                                                SiparisCihazHareket.SiparisCihazId = SiparisCihaz.Id
				                                                ORDER BY
					                                                SiparisCihazHareket.IslemZamani DESC) AS SCH
                                                WHERE
	                                                SiparisCihaz.SiparisId = {siparis.Id.ToString()}";

                    var resultSiparisCihaz = await connection.QueryAsync<SiparisCihazDTO>(QSiparisCihaz);

                    siparis.SiparisCihazList = new List<SiparisCihazDTO>();

                    siparis.SiparisCihazList.AddRange(resultSiparisCihaz.ToList());

                    siparis.DetailCount = siparis.SiparisCihazList.Count();
                }

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(QSiparis, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = resultSiparis.ToList();
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

        public async Task<ServiceResponse<List<SiparisCihazTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<SiparisCihazTakipDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<SiparisCihazTakipDTO>("SPSelectMusteriTakip @GSM, @Mail, 'Siparis'", filterDTO);

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

        public async Task<ServiceResponse<string>> InsertSiparis(SiparisDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QSiparis = $@"  INSERT INTO Siparis
									        (GSM,
									        Mail,
									        AdSoyadSiparis,
									        TCKimlikNoSiparis,
									        IlIlceIdSiparis,
									        AdresSiparis,
									        AdSoyadFatura,
									        TCKimlikNoFatura,
									        UnvanFatura,
									        VergiDairesiFatura,
									        VergiNoFatura,
									        IlIlceIdFatura,
									        AdresFatura,
                                            KargoFirmasiId,
                                            KargoUcreti,
									        SiparisZamani,
                                            VerifyEnrollmentRequestId,
                                            CVV)
								        VALUES
									        (@GSM,
									        @Mail,
									        @AdSoyadSiparis,
									        @TCKimlikNoSiparis,
									        @IlceIdSiparis,
									        @AdresSiparis,
									        @AdSoyadFatura,
									        @TCKimlikNoFatura,
									        @UnvanFatura,
									        @VergiDairesiFatura,
									        @VergiNoFatura,
									        (CASE WHEN @IlceIdFatura = 0 THEN NULL ELSE @IlceIdFatura END),
									        @AdresFatura,
                                            (SELECT TOP 1 KargoFirmasiId FROM Ayar),
                                            @KargoUcreti,
									        GETDATE(),
                                            @VerifyEnrollmentRequestId,
                                            @KrediKartiCVV)

								        SELECT @@IDENTITY";

                var resultSiparis = await connection.ExecuteScalarAsync<string>(QSiparis, dto);

                string QSiparisCihaz = $@"  INSERT INTO SiparisCihaz
                                                (SiparisId,
										        CihazId,
										        Fiyat,
										        KdvOrani)
                                            VALUES
                                                (@SiparisId,
										        @ModelId,
										        @Fiyat,
										        @KdvOrani)

                                            SELECT @@IDENTITY";

                foreach (var dtoSiparisCihaz in dto.SiparisCihazList)
                {
                    dtoSiparisCihaz.SiparisId = Convert.ToInt32(resultSiparis);

                    for (int i = 1; i <= dtoSiparisCihaz.Miktar; i++)
                    {
                        await connection.ExecuteScalarAsync<string>(QSiparisCihaz, dtoSiparisCihaz);
                    }
                }

                R.Value = resultSiparis;
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

        public async Task<ServiceResponse<string>> InsertOrUpdateSiparis(SiparisDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
								BEGIN
									INSERT INTO Siparis
										(GSM,
										Mail,
										AdSoyadSiparis,
										TCKimlikNoSiparis,
										IlIlceIdSiparis,
										AdresSiparis,
										AdSoyadFatura,
										TCKimlikNoFatura,
										UnvanFatura,
										VergiDairesiFatura,
										VergiNoFatura,
										IlIlceIdFatura,
										AdresFatura,
										SiparisZamani)
									VALUES
										(@GSM,
										@Mail,
										@AdSoyadSiparis,
										@TCKimlikNoSiparis,
										(CASE WHEN @IlceIdSiparis = 0 THEN NULL ELSE @IlceIdSiparis END),
										@AdresSiparis,
										@AdSoyadFatura,
										@TCKimlikNoFatura,
										@UnvanFatura,
										@VergiDairesiFatura,
										@VergiNoFatura,
										(CASE WHEN @IlceIdFatura = 0 THEN NULL ELSE @IlceIdFatura END),
										@AdresFatura,
										GETDATE())

									SELECT @@IDENTITY
								END ELSE
								BEGIN
									UPDATE Siparis SET
										GSM = @GSM,
										Mail = @Mail,
										AdSoyadSiparis = @AdSoyadSiparis,
										TCKimlikNoSiparis = @TCKimlikNoSiparis,
										IlIlceIdSiparis = (CASE WHEN @IlceIdSiparis = 0 THEN NULL ELSE @IlceIdSiparis END),
										AdresSiparis = @AdresSiparis,
										AdSoyadFatura = @AdSoyadFatura,
										TCKimlikNoFatura = @TCKimlikNoFatura,
										UnvanFatura = @UnvanFatura,
										VergiDairesiFatura = @VergiDairesiFatura,
										VergiNoFatura = @VergiNoFatura,
										IlIlceIdFatura = (CASE WHEN @IlceIdFatura = 0 THEN NULL ELSE @IlceIdFatura END),
										AdresFatura = @AdresFatura
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

        public async Task<ServiceResponse<string>> DeleteSiparis(List<SiparisDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($@"	DELETE FROM SiparisCihaz WHERE SiparisId = {dto.Id}
														DELETE FROM Siparis WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<string>> InsertOrUpdateSiparisCihaz(SiparisCihazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO SiparisCihaz
                                        (SiparisId,
										CihazId,
                                        SeriNo,
										Fiyat,
										KdvOrani)
                                    VALUES
                                        (@SiparisId,
										@ModelId,
                                        @SeriNo,
										@Fiyat,
										@KdvOrani)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE SiparisCihaz SET
		                                CihazId = @ModelId,
                                        SeriNo = @SeriNo,
		                                Fiyat = @Fiyat,
		                                KdvOrani = @KdvOrani
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

        public async Task<ServiceResponse<string>> DeleteSiparisCihaz(List<SiparisCihazDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($@"   DELETE FROM SiparisCihazHareket WHERE SiparisCihazId = {dto.Id}
                                                        DELETE FROM SiparisCihaz WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<List<SiparisCihazHareketDTO>>> GetSiparisCihazHareket(int SiparisCihazId, int KisiId)
        {
            var R = new ServiceResponse<List<SiparisCihazHareketDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  SELECT
	                                0 - SiparisCihaz.Id AS Id,
	                                SiparisCihaz.Id AS SiparisCihazId,
	                                Siparis.SiparisZamani AS IslemZamani,
	                                'Sipariş verildi' AS SiparisCihazHareketTuru,
	                                'Sipariş verildi' AS Aciklama
                                FROM
	                                SiparisCihaz
	                                LEFT JOIN Siparis ON SiparisCihaz.SiparisId = Siparis.Id
                                WHERE
	                                SiparisCihaz.Id = {SiparisCihazId}

                                UNION ALL

                                SELECT
	                                SiparisCihazHareket.Id,
	                                SiparisCihazHareket.SiparisCihazId,
	                                SiparisCihazHareket.IslemZamani,
	                                SiparisCihazHareketTuru.SiparisCihazHareketTuru,
	                                ISNULL(CASE
        		                                WHEN SiparisCihazHareketTuru.SiparisCihazHareketTuru = 'Kargolandı' THEN KargoFirmasi.KargoFirmasi + ' ile kargolandı. Takip no: ' + SiparisCihazHareket.KargoTakipNo
		                                        WHEN SiparisCihazHareketTuru.SiparisCihazHareketTuru = 'Faturalandı' THEN 'Fatura No: ' + SiparisCihazHareket.FaturaNo
	                                        END, '') +
                                    ISNULL(' ' + SiparisCihazHareket.Aciklama, '') AS Aciklama
                                FROM
	                                SiparisCihazHareket
	                                LEFT JOIN SiparisCihazHareketTuru ON SiparisCihazHareket.SiparisCihazHareketTuruId = SiparisCihazHareketTuru.Id
	                                LEFT JOIN KargoFirmasi ON SiparisCihazHareket.KargoFirmasiId = KargoFirmasi.Id
                                WHERE
	                                SiparisCihazHareket.SiparisCihazId = {SiparisCihazId}";

                var result = await connection.QueryAsync<SiparisCihazHareketDTO>(Q);

                R.Value = result.ToList();
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, SiparisCihazId, 0);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertSiparisCihazHareket(SiparisCihazHareketDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  INSERT INTO SiparisCihazHareket
		                            (SiparisCihazId,
		                            SiparisCihazHareketTuruId,
		                            IslemZamani,
		                            KargoFirmasiId,
		                            KargoTakipNo,
		                            FaturaNo,
                                    Aciklama)
	                            VALUES
		                            (@SiparisCihazId,
		                            @SiparisCihazHareketTuruId,
		                            @IslemZamani,
		                            (CASE WHEN @KargoFirmasiId = 0 THEN NULL ELSE @KargoFirmasiId END),
		                            @KargoTakipNo,
		                            @FaturaNo,
                                    @Aciklama)

	                            SELECT @@IDENTITY";

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