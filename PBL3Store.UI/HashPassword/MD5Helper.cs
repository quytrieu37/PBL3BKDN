using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace PBL3Store.UI.HashPassword
{
    public class MD5Helper
    {
        public static string HashMD5(string Pass)
        {
            MD5 md5Hash = MD5.Create();
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(Pass));
            StringBuilder sb = new StringBuilder();
            for(int i=0; i< data.Length;++i)
            {
                sb.Append(data[i].ToString("x2"));
            }
            return sb.ToString();
        }
        public static bool VerifyPass(string HassPass, string input)
        {
            string hashInput = HashMD5(input);
            StringComparer compare = StringComparer.OrdinalIgnoreCase;
            if (0 == compare.Compare(hashInput, HassPass))
            {
                return true;
            }
            return false;
        }
    }
}