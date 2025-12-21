using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace RADLAB.Business.Utils
{
    public class IslemLog
    {
        public static async Task<string> Logla(IConfiguration configuration, int kisiId, Type methodBaseType, string exceptionMessage, object icerik, int ilgiliId)
        {
            bool debug = false;
#if DEBUG
            debug = true;
#endif

            string CS = configuration.GetConnectionString(debug ? "Debug" : "Release");

            string islemAdi = methodBaseType.Name.Split('>')[0].Replace("<", "");

            var IcerikString = JsonConvert.SerializeObject(icerik);

            using var connection = new SqlConnection(CS);

            string Q = $@"  INSERT INTO IslemLog
	                            (KisiId, Zaman, IP, IslemAdi, Icerik, ExceptionMessage, IlgiliId)
                            VALUES
	                            ((CASE WHEN @KisiId = 0 THEN NULL ELSE @KisiId END), GETDATE(), '', @IslemAdi, @Icerik, @ExceptionMessage, @IlgiliId)

                            SELECT @@IDENTITY";

            var result = await connection.ExecuteScalarAsync<string>(Q, new { KisiId = kisiId, IslemAdi = islemAdi, ExceptionMessage = exceptionMessage, Icerik = IcerikString, IlgiliId = ilgiliId });

            if (string.IsNullOrEmpty(exceptionMessage))
            {
                return result.ToString();
            }
            else if (exceptionMessage.StartsWith("Hata:"))
            {
                return exceptionMessage;
            }
            else
            {
                return result.ToString() + " Id nolu exception oluştu";
            }
        }
    }
}