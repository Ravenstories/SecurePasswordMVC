using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace SignalRClient.Functions
{
    public static class Aes_EncryptDecrypt
    {
        /// <summary>
        /// Classic cookie cutter Encrypt and Decrypt AES functions. 
        /// Used on both server and client.  
        /// </summary>
        /// <param name="algorithm">The AES object</param>
        /// <param name="text">The text the user or server sends back and forth</param>
        /// <returns></returns>
        public static string Encrypt(SymmetricAlgorithm algorithm, string text)
        {
            byte[] array;

            ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter sw = new StreamWriter((Stream)cs))
                    {
                        sw.Write(text);
                    }
                    array = ms.ToArray();
                }
            }
            return Convert.ToBase64String(array);
        }
        public static string Decrypt(SymmetricAlgorithm algorithm, byte[] text)
        {
            ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

            using (MemoryStream ms = new MemoryStream(text))
            {
                using (CryptoStream cs = new CryptoStream((Stream)ms, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader sr = new StreamReader((Stream)cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }
    }
}
