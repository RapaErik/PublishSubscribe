using Newtonsoft.Json.Linq;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Contracts
{
    public static class CryptoAESManager
    {
        private readonly static byte[] _key = new byte[] { 0xB3, 0x74, 0xA2, 0x6A, 0x71, 0x49, 0x04, 0xB3, 0x74, 0xA2, 0x6A, 0x71, 0x49, 0x04, 0x49, 0x04 };

        public static string Encrypt(string plainText)
        {
            byte[] iv = new byte[16];
            byte[] array;

            using(Aes aes= Aes.Create())
            {
                aes.Key =_key;
                aes.IV = iv;
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key,aes.IV) ;
                using (MemoryStream ms= new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms,encryptor, CryptoStreamMode.Write)) 
                    {
                        using (StreamWriter sw = new StreamWriter(cryptoStream))
                        {
                            sw.Write(plainText);
                        }
                        array = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(array);
        }
        public static string Dencrypt(string encryptedText)
        {
            byte[] iv = new byte[16];
            byte[] buffer= Convert.FromBase64String(encryptedText);

            string result = default;

            using (Aes aes = Aes.Create())
            {
                aes.Key =_key;
                aes.IV = iv;
                ICryptoTransform dencryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(ms, dencryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cryptoStream))
                        {
                            result = sr.ReadToEnd();
                        }
                    }
                }
            }
            return result;
        }



    }
}
