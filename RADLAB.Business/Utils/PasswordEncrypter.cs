using System;
using System.Collections.Generic;
using System.Text;

namespace RADLAB.Business.Utils
{
    public class PasswordEncrypter
    {
        public static string Encrypt(string Password)
        {
            // Kendi algoritmanızı kullanabilirsiniz.
            // Ben Base64 olarak kullanıyorum


            var plainTextBytes = Encoding.UTF8.GetBytes(Password);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}