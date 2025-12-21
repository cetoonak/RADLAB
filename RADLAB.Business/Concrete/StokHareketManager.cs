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
    public class StokHareketManager : IStokHareketManager
    {
        private readonly IConfiguration configuration;
        private readonly string CS;

        public StokHareketManager(IConfiguration _configuration)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            configuration = _configuration;

            CS = configuration.GetConnectionString(debug ? "Debug" : "Release");
        }

        public async Task<ServiceResponse<List<StokHareketDTO>>> GetStokHareketList(StokHareketFilterDTO filterDTO, int KisiId)
        {
            try
            {
                string tarih1 = filterDTO.HareketTarihiAraligi.Split('-')[0].Trim();
                string tarih2 = filterDTO.HareketTarihiAraligi.Split('-')[1].Trim();

                int tarih1Gun = Convert.ToInt16(tarih1.Split('.')[0]);
                int tarih1Ay = Convert.ToInt16(tarih1.Split('.')[1]);
                int tarih1Yil = Convert.ToInt16(tarih1.Split('.')[2]);

                int tarih2Gun = Convert.ToInt16(tarih2.Split('.')[0]);
                int tarih2Ay = Convert.ToInt16(tarih2.Split('.')[1]);
                int tarih2Yil = Convert.ToInt16(tarih2.Split('.')[2]);

                filterDTO.HareketTarihi1 = Convert.ToInt32((new DateTime(tarih1Yil, tarih1Ay, tarih1Gun)).ToOADate());
                filterDTO.HareketTarihi2 = Convert.ToInt32((new DateTime(tarih2Yil, tarih2Ay, tarih2Gun)).ToOADate());
            }
            catch
            {
                filterDTO.HareketTarihi1 = 0;
                filterDTO.HareketTarihi2 = 0;
            }

            var R = new ServiceResponse<List<StokHareketDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string QStokHareket = $@"SPSelectStokHareket
                                                @StokHareketTuruId,
							                    @HareketTarihi1,
							                    @HareketTarihi2,
							                    @EvrakNo,
							                    @Aciklama,
							                    @Cihaz,
							                    @Order,
							                    @Id,
							                    @PageNo,
							                    @PageSize,
							                    @DonusTipi";

                var resultStokHareket = await connection.QueryAsync<StokHareketDTO>(QStokHareket, filterDTO);

                foreach (var stokHareket in resultStokHareket)
                {
                    string QStokHareketCihaz = $@"  SELECT
	                                                    StokHareketCihaz.*,
                                                        Model.ParentId AS MarkaId,
                                                        StokHareketCihaz.CihazId AS ModelId,
	                                                    Marka.Cihaz AS Marka,
	                                                    Model.Cihaz AS Model
                                                    FROM
	                                                    StokHareketCihaz
	                                                    LEFT JOIN StokHareket ON StokHareketCihaz.StokHareketId = StokHareket.Id
	                                                    LEFT JOIN Cihaz Model ON StokHareketCihaz.CihazId = Model.Id
	                                                    LEFT JOIN Cihaz Marka ON Model.ParentId = Marka.Id
                                                    WHERE
	                                                    StokHareketCihaz.StokHareketId = 0{stokHareket.Id.ToString()}";

                    var resultStokHareketCihaz = await connection.QueryAsync<StokHareketCihazDTO>(QStokHareketCihaz);

                    stokHareket.StokHareketCihazList = new List<StokHareketCihazDTO>();

                    stokHareket.StokHareketCihazList.AddRange(resultStokHareketCihaz.ToList());

                    stokHareket.DetailCount = stokHareket.StokHareketCihazList.Count();
                }

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(QStokHareket, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = resultStokHareket.ToList();
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

        public async Task<ServiceResponse<string>> InsertOrUpdateStokHareket(StokHareketDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
								BEGIN
									INSERT INTO StokHareket
										(StokHareketTuruId,
										HareketTarihi,
										EvrakNo,
										EvrakTarihi,
										Aciklama)
									VALUES
										(@StokHareketTuruId,
										@HareketTarihi,
										@EvrakNo,
										(CASE WHEN YEAR(@EvrakTarihi) <= 1900 THEN NULL ELSE @EvrakTarihi END),
										@Aciklama)

									SELECT @@IDENTITY
								END ELSE
								BEGIN
									UPDATE StokHareket SET
										StokHareketTuruId = @StokHareketTuruId,
										HareketTarihi = @HareketTarihi,
										EvrakNo = @EvrakNo,
										EvrakTarihi = (CASE WHEN YEAR(@EvrakTarihi) <= 1900 THEN NULL ELSE @EvrakTarihi END),
										Aciklama = @Aciklama
									WHERE
										Id = @Id

									SELECT @Id
								END";

                var result = await connection.ExecuteScalarAsync<string>(Q, dto);

                if (dto.Id == 0)
                {
                    foreach (var stokHareketCihaz in dto.StokHareketCihazList)
                    {
                        string QStokHareketCihaz = $@"  INSERT INTO StokHareketCihaz
                                                            (StokHareketId,
										                    CihazId,
                                                            Miktar)
                                                        VALUES
                                                            ({result},
										                    {stokHareketCihaz.ModelId},
                                                            {stokHareketCihaz.Miktar})

                                                        SELECT @@IDENTITY";

                        await connection.ExecuteScalarAsync<string>(QStokHareketCihaz, dto);
                    }
                }

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

        public async Task<ServiceResponse<string>> DeleteStokHareket(List<StokHareketDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($@"	DELETE FROM StokHareketCihaz WHERE StokHareketId = {dto.Id}
														DELETE FROM StokHareket WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<string>> InsertOrUpdateStokHareketCihaz(StokHareketCihazDTO dto, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"  IF @Id = 0
                                BEGIN
                                    INSERT INTO StokHareketCihaz
                                        (StokHareketId,
										CihazId,
                                        Miktar)
                                    VALUES
                                        (@StokHareketId,
										@ModelId,
                                        @Miktar)

                                    SELECT @@IDENTITY
                                END ELSE
                                BEGIN
                                    UPDATE StokHareketCihaz SET
		                                CihazId = @ModelId,
                                        Miktar = @Miktar
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

        public async Task<ServiceResponse<string>> DeleteStokHareketCihaz(List<StokHareketCihazDTO> dtos, int KisiId)
        {
            var R = new ServiceResponse<string>();

            try
            {
                using var connection = new SqlConnection(CS);

                foreach (var dto in dtos)
                {
                    await connection.ExecuteAsync($@"   DELETE FROM StokHareketCihazHareket WHERE StokHareketCihazId = {dto.Id}
                                                        DELETE FROM StokHareketCihaz WHERE Id = {dto.Id}");
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

        public async Task<ServiceResponse<List<StokHareketleriDTO>>> GetStokHareketleriList(StokHareketleriFilterDTO filterDTO, int KisiId)
        {
            try
            {
                string tarih1 = filterDTO.HareketTarihiAraligi.Split('-')[0].Trim();
                string tarih2 = filterDTO.HareketTarihiAraligi.Split('-')[1].Trim();

                int tarih1Gun = Convert.ToInt16(tarih1.Split('.')[0]);
                int tarih1Ay = Convert.ToInt16(tarih1.Split('.')[1]);
                int tarih1Yil = Convert.ToInt16(tarih1.Split('.')[2]);

                int tarih2Gun = Convert.ToInt16(tarih2.Split('.')[0]);
                int tarih2Ay = Convert.ToInt16(tarih2.Split('.')[1]);
                int tarih2Yil = Convert.ToInt16(tarih2.Split('.')[2]);

                filterDTO.HareketTarihi1 = Convert.ToInt32((new DateTime(tarih1Yil, tarih1Ay, tarih1Gun)).ToOADate());
                filterDTO.HareketTarihi2 = Convert.ToInt32((new DateTime(tarih2Yil, tarih2Ay, tarih2Gun)).ToOADate());
            }
            catch
            {
                filterDTO.HareketTarihi1 = 0;
                filterDTO.HareketTarihi2 = 0;
            }

            var R = new ServiceResponse<List<StokHareketleriDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectStokHareketleri
                                @MarkaId,
                                @ModelId,
                                @StokHareketTuruId,
							    @HareketTarihi1,
							    @HareketTarihi2,
							    @Order,
							    @PageNo,
							    @PageSize,
							    @DonusTipi";

                var resultStokHareket = await connection.QueryAsync<StokHareketleriDTO>(Q, filterDTO);

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(Q, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = resultStokHareket.ToList();
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

        public async Task<ServiceResponse<List<StokMiktarlariDTO>>> GetStokMiktarlariList(StokMiktarlariFilterDTO filterDTO, int KisiId)
        {
            var R = new ServiceResponse<List<StokMiktarlariDTO>>();

            try
            {
                using var connection = new SqlConnection(CS);

                string Q = $@"SPSelectStokMiktarlari
                                @MarkaId,
                                @ModelId,
							    @Order,
							    @PageNo,
							    @PageSize,
							    @DonusTipi";

                var resultStokHareket = await connection.QueryAsync<StokMiktarlariDTO>(Q, filterDTO);

                if (filterDTO.DonusTipi == 0)
                {
                    filterDTO.DonusTipi = 1;

                    var RC = await connection.QuerySingleOrDefaultAsync<int>(Q, filterDTO);

                    R.RowCount = RC;
                }

                R.Value = resultStokHareket.ToList();
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
    }
}