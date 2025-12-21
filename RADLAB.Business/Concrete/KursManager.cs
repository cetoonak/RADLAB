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
    public class KursManager : IKursManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public KursManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<List<KursDTO>>> GetKursList(KursFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<KursDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QKurs = $@"SPSelectKurs
	                                    @EgitimId,
	                                    @KursOrtamiId,
	                                    @Yil,
	                                    @Durum,
	                                    @Id,
	                                    @PageNo,
	                                    @PageSize,
	                                    @DonusTipi";

                var resultKurs = await connection.QueryAsync<KursDTO>(QKurs, filterDTO);

                foreach (var kurs in resultKurs)
                {
                    string QKursiyer = $@"SELECT
                                                ROW_NUMBER() OVER (PARTITION BY BeklemedeAsilYedekIptal ORDER BY BeklemedeAsilYedekIptal, Ad, Soyad) AS Sira,
												Id,
												TCKimlikNo,
												Ad,
												Soyad,
												DogumTarihi,
												Cinsiyet,
												Mail,
												TelefonCep,
												Meslek,
												Okul,
												BeklemedeAsilYedekIptal,
												CASE Kursiyer.Cinsiyet
													WHEN 1 THEN 'Erkek'
													WHEN 2 THEN 'Kadın'
												END AS CinsiyetString,
	                                            CASE BeklemedeAsilYedekIptal
		                                            WHEN 1 THEN 'Beklemede'
		                                            WHEN 2 THEN 'Asil'
		                                            WHEN 3 THEN 'Yedek'
		                                            WHEN 4 THEN 'İptal'
	                                            END AS BeklemedeAsilYedekIptalString
                                            FROM
	                                            Kursiyer
                                            WHERE
	                                            KursId = {kurs.Id.ToString()}";

                    var resultKursiyer = await connection.QueryAsync<KursiyerDTO>(QKursiyer);

                    kurs.KursiyerList = new List<KursiyerDTO>();

                    kurs.KursiyerList.AddRange(resultKursiyer.ToList());

                    kurs.DetailCount = kurs.KursiyerList.Count();
                }

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(QKurs, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = resultKurs.ToList();
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

        public async Task<ServiceResponse<string>> InsertOrUpdateKurs(KursDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Kurs
                                        (Sira,
                                        EgitimId,
		                                KursOrtamiId,
		                                BasvuruBaslangici,
		                                BasvuruBitisi,
		                                EgitimBaslangici,
		                                EgitimBitisi,
		                                YayindanKalkis,
		                                Sure,
		                                Fiyat,
		                                KdvOraniId,
		                                KontenjanAsil,
		                                KontenjanYedek,
		                                Aciklama,
		                                KursYeri)
                                    VALUES
                                        (@Sira,
		                                @EgitimId,
		                                @KursOrtamiId,
		                                @BasvuruBaslangici,
		                                @BasvuruBitisi,
		                                @EgitimBaslangici,
		                                @EgitimBitisi,
		                                @YayindanKalkis,
		                                @Sure,
		                                @Fiyat,
		                                @KdvOraniId,
		                                @KontenjanAsil,
		                                @KontenjanYedek,
		                                @Aciklama,
		                                @KursYeri)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Kurs SET
										EgitimId = @EgitimId,
                                        Sira = @Sira,
		                                KursOrtamiId = @KursOrtamiId,
		                                BasvuruBaslangici = @BasvuruBaslangici,
		                                BasvuruBitisi = @BasvuruBitisi,
		                                EgitimBaslangici = @EgitimBaslangici,
		                                EgitimBitisi = @EgitimBitisi,
		                                YayindanKalkis = @YayindanKalkis,
		                                Sure = @Sure,
		                                Fiyat = @Fiyat,
		                                KdvOraniId = @KdvOraniId,
		                                KontenjanAsil = @KontenjanAsil,
		                                KontenjanYedek = @KontenjanYedek,
		                                Aciklama = @Aciklama,
		                                KursYeri = @KursYeri
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

        public async Task<ServiceResponse<string>> DeleteKurs(List<KursDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($@"	DELETE FROM Kursiyer WHERE KursId = {{dto.Id}}""
														DELETE FROM Kurs WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<List<KursYayinDTO>>> GetKursYayinList(int Id, int KisiId)
        {
            var R = new ServiceResponse<List<KursYayinDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"	SELECT
									Kurs.Id,
									Egitim.Egitim,
									Egitim.Baslik,
									Egitim.Kisaltma,
									Egitim.Amac,
									Egitim.Sart,
									KursOrtami.KursOrtami,
									CONVERT(VARCHAR(10), Kurs.BasvuruBaslangici, 104) + '-' + CONVERT(VARCHAR(10), Kurs.BasvuruBitisi, 104) AS BasvuruDonemi,
									CONVERT(VARCHAR(10), Kurs.EgitimBaslangici, 104) + '-' + CONVERT(VARCHAR(10), Kurs.EgitimBitisi, 104) AS EgitimDonemi,
									Kurs.Sure,
									Kurs.KursYeri,
									CASE
										WHEN CONVERT(DATE, GETDATE()) BETWEEN Kurs.EgitimBaslangici AND Kurs.EgitimBitisi THEN 'Eğitim Süresi İçinde'
										WHEN CONVERT(DATE, GETDATE()) BETWEEN Kurs.BasvuruBaslangici AND Kurs.BasvuruBitisi THEN 'Başvuru Dönemi İçinde'
										WHEN CONVERT(DATE, GETDATE()) >= Kurs.YayindanKalkis THEN 'Yayında değil'
										WHEN CONVERT(DATE, GETDATE()) < Kurs.BasvuruBaslangici THEN 'Başvuru Dönemi Başlamadı'
										WHEN CONVERT(DATE, GETDATE()) < Kurs.EgitimBaslangici THEN 'Başvuru Dönemi Bitti, Kurs Başlamadı'
										WHEN CONVERT(DATE, GETDATE()) > Kurs.EgitimBitisi THEN 'Eğitim Süresi Bitti'
									END AS Durum
								FROM
									Kurs
									LEFT JOIN Egitim ON Kurs.EgitimId = Egitim.Id
									LEFT JOIN KursOrtami ON Kurs.KursOrtamiId = KursOrtami.Id
								WHERE
									Kurs.YayindanKalkis > CONVERT(DATE, GETDATE())
									AND (Kurs.Id = {Id} OR {Id} = 0)";

                var result = await connection.QueryAsync<KursYayinDTO>(Q);

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

        public async Task<ServiceResponse<List<KursiyerTakipDTO>>> GetMusteriTakip(MusteriTakipFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<KursiyerTakipDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                var result = await connection.QueryAsync<KursiyerTakipDTO>("SPSelectMusteriTakip @GSM, @Mail, 'Egitim'", filterDTO);

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

        public async Task<ServiceResponse<KursiyerDTO>> GetKursiyer(int Id, int KisiId)
        {
            var R = new ServiceResponse<KursiyerDTO>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SELECT
	                                Kursiyer.*,
                                    Kursiyer.IlIlceIdAdres AS IlceIdAdres,
                                    IlceAdres.ParentId AS IlIdAdres,
                                    Kursiyer.IlIlceIdFirma AS IlceIdFirma,
                                    IlceFirma.ParentId AS IlIdFirma,
                                    Kursiyer.IlIlceIdTercih AS IlIdTercih,
	                                CASE Kursiyer.Cinsiyet
		                                WHEN 1 THEN 'Erkek'
		                                WHEN 2 THEN 'Kadın'
	                                END AS CinsiyetString,
	                                CASE Kursiyer.BeklemedeAsilYedekIptal
		                                WHEN 1 THEN 'Beklemede'
		                                WHEN 2 THEN 'Asil'
		                                WHEN 3 THEN 'Yedek'
		                                WHEN 4 THEN 'İptal'
	                                END AS BeklemedeAsilYedekIptalString,
	                                IlAdres.IlIlce AS IlAdres,
	                                IlceAdres.IlIlce AS IlceAdres,
	                                IlFirma.IlIlce AS IlFirma,
	                                IlceFirma.IlIlce AS IlceFirma,
	                                IlTercih.IlIlce AS IlTercih
                                FROM
	                                Kursiyer
	                                LEFT JOIN IlIlce IlceAdres ON Kursiyer.IlIlceIdAdres = IlceAdres.Id
	                                LEFT JOIN IlIlce IlAdres ON IlceAdres.ParentId = IlAdres.Id
	                                LEFT JOIN IlIlce IlceFirma ON Kursiyer.IlIlceIdFirma = IlceFirma.Id
	                                LEFT JOIN IlIlce IlFirma ON IlceFirma.ParentId = IlFirma.Id
	                                LEFT JOIN IlIlce IlTercih ON Kursiyer.IlIlceIdTercih = IlTercih.Id
                                WHERE
	                                Kursiyer.Id = {Id.ToString()}";

                var result = await connection.QuerySingleAsync<KursiyerDTO>(Q);

                R.Value = result;
            }
            catch (Exception ex)
            {
                R.Success = false;
                R.Message = ex.Message;
            }

            if (!R.Success)
            {
                var logReturn = await IslemLog.Logla(configuration, KisiId, MethodBase.GetCurrentMethod().DeclaringType, R.Message, new object(), Id);
                R.Message = logReturn;
            }

            return R;
        }

        public async Task<ServiceResponse<string>> InsertOrUpdateKursiyer(KursiyerDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO Kursiyer
                                        (KursId,
		                                TCKimlikNo,
		                                Ad,
		                                Soyad,
		                                DogumTarihi,
		                                Cinsiyet,
		                                Mail,
		                                TelefonCep,
		                                Adres,
		                                Meslek,
		                                Okul,
		                                Diploma,
		                                DiplomaDosyaAdi,
		                                DiplomaUzanti,
		                                KimlikOn,
		                                KimlikOnDosyaAdi,
		                                KimlikOnUzanti,
		                                KimlikArka,
		                                KimlikArkaDosyaAdi,
		                                KimlikArkaUzanti,
		                                Aciklama,
		                                BeklemedeAsilYedekIptal,
		                                Fiyat,
		                                Firma,
		                                VergiDairesi,
		                                VergiNo,
		                                AdresFirma,
		                                IlIlceIdAdres,
		                                IlIlceIdFirma,
		                                IlIlceIdTercih,
		                                DekontNo,
		                                Dekont,
		                                DekontDosyaAdi,
		                                DekontUzanti)
                                    VALUES
                                        (@KursId,
		                                @TCKimlikNo,
		                                @Ad,
		                                @Soyad,
		                                @DogumTarihi,
		                                @Cinsiyet,
		                                @Mail,
		                                @TelefonCep,
		                                @Adres,
		                                @Meslek,
		                                @Okul,
		                                @Diploma,
		                                @DiplomaDosyaAdi,
		                                @DiplomaUzanti,
		                                @KimlikOn,
		                                @KimlikOnDosyaAdi,
		                                @KimlikOnUzanti,
		                                @KimlikArka,
		                                @KimlikArkaDosyaAdi,
		                                @KimlikArkaUzanti,
		                                @Aciklama,
		                                @BeklemedeAsilYedekIptal,
		                                @Fiyat,
		                                @Firma,
		                                @VergiDairesi,
		                                @VergiNo,
		                                @AdresFirma,
		                                (CASE WHEN @IlIlceIdAdres = 0 THEN NULL ELSE @IlIlceIdAdres END),
		                                (CASE WHEN @IlIlceIdFirma = 0 THEN NULL ELSE @IlIlceIdFirma END),
		                                (CASE WHEN @IlIdTercih = 0 THEN NULL ELSE @IlIdTercih END),
		                                @DekontNo,
		                                @Dekont,
		                                @DekontDosyaAdi,
		                                @DekontUzanti)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE Kursiyer SET
		                                TCKimlikNo = @TCKimlikNo,
		                                Ad = @Ad,
		                                Soyad = @Soyad,
		                                DogumTarihi = @DogumTarihi,
		                                Cinsiyet = @Cinsiyet,
		                                Mail = @Mail,
		                                TelefonCep = @TelefonCep,
		                                Adres = @Adres,
		                                Meslek = @Meslek,
		                                Okul = @Okul,
		                                Diploma = @Diploma,
		                                DiplomaDosyaAdi = @DiplomaDosyaAdi,
		                                DiplomaUzanti = @DiplomaUzanti,
		                                KimlikOn = @KimlikOn,
		                                KimlikOnDosyaAdi = @KimlikOnDosyaAdi,
		                                KimlikOnUzanti = @KimlikOnUzanti,
		                                KimlikArka = @KimlikArka,
		                                KimlikArkaDosyaAdi = @KimlikArkaDosyaAdi,
		                                KimlikArkaUzanti = @KimlikArkaUzanti,
		                                Aciklama = @Aciklama,
		                                BeklemedeAsilYedekIptal = @BeklemedeAsilYedekIptal,
		                                Fiyat = @Fiyat,
		                                Firma = @Firma,
		                                VergiDairesi = @VergiDairesi,
		                                VergiNo = @VergiNo,
		                                AdresFirma = @AdresFirma,
		                                IlIlceIdAdres = (CASE WHEN @IlIlceIdAdres = 0 THEN NULL ELSE @IlIlceIdAdres END),
		                                IlIlceIdFirma = (CASE WHEN @IlIlceIdFirma = 0 THEN NULL ELSE @IlIlceIdFirma END),
		                                IlIlceIdTercih = (CASE WHEN @IlIdTercih = 0 THEN NULL ELSE @IlIdTercih END),
		                                DekontNo = @DekontNo,
		                                Dekont = @Dekont,
		                                DekontDosyaAdi = @DekontDosyaAdi,
		                                DekontUzanti = @DekontUzanti
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

        public async Task<ServiceResponse<string>> DeleteKursiyer(List<KursiyerDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($"DELETE FROM Kursiyer WHERE Id = {dto.Id}");
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