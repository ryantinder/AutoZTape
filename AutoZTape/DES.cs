using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
namespace AutoZTape
{
    public class DES
    {
        public static string decrypt(string target)

        {
            //Configuration > Section > Class > Text
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            ConnectionStringsSection csSection = config.ConnectionStrings;
            var settings = ConfigurationManager.ConnectionStrings[target];
            string connection = csSection.ConnectionStrings[target].ConnectionString;

            byte[] inputArray = Convert.FromBase64String(connection);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            string connectionstring = Encoding.UTF8.GetString(resultArray);
            Console.WriteLine("Descryption Result: " + connectionstring);
            return connectionstring;
        }
        public static string decryptString(string src)

        {
            
            byte[] inputArray = Convert.FromBase64String(src);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();

            string connectionstring = Encoding.UTF8.GetString(resultArray);
            Console.WriteLine("Descryption Result: " + connectionstring);
            return connectionstring;
        }
        public static string encryptString(string src)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(src);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes("sblw-3hn8-sqoy19");
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            string output = Convert.ToBase64String(resultArray, 0, resultArray.Length);


            return output;
        }
    }
}
