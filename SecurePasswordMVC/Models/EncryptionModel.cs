using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CodeActions;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace SecurePasswordMVC.Models
{
    public class EncryptionModel
    {
        public static string? savedText;
        public static TimeSpan? timed;

        static public SymmetricAlgorithm Triple_DES = TripleDES.Create();
        static public SymmetricAlgorithm AES = Aes.Create();

        public List<SelectListItem> CipherAlgorithms = new()
        { 
            new SelectListItem { Text = "Triple_DES", Value = "Triple_DES" },
            new SelectListItem { Text = "AES", Value = "AES" }
        };

        [Required]
        public string Key { get; set; }
        [Required]
        public string IV { get; set; }
        [Required]
        public string UserChosenAlgorithm { get; set; }
        public SymmetricAlgorithm? Algorithm { get; set; }
        public string? PlainText { get; set; }
        public string? CipherText { get; set; }
        [Required]
        public CipherMode Cipher { get; set; }
        public PaddingMode Padding { get; set; }

        public static string Encrypt(EncryptionModel encryptionModel, SymmetricAlgorithm algorithm, string text)
        {
            byte[] array;
            
            algorithm.Key = Encoding.UTF8.GetBytes(encryptionModel.Key);
            algorithm.IV = Encoding.UTF8.GetBytes(encryptionModel.IV);
            
            ICryptoTransform encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);
            
            using(MemoryStream ms = new MemoryStream())
            {
                using(CryptoStream cs = new CryptoStream((Stream)ms, encryptor, CryptoStreamMode.Write))
                {
                    using(StreamWriter sw = new StreamWriter((Stream)cs))
                    {
                        sw.Write(text);
                    }
                    array = ms.ToArray();
                }
            }
            return Convert.ToBase64String(array);
        }
        public static string Decrypt(EncryptionModel encryptionModel, SymmetricAlgorithm algorithm, string text)
        {
            byte[] buffer = Convert.FromBase64String(text);
            
            algorithm.Key = Encoding.UTF8.GetBytes(encryptionModel.Key);
            algorithm.IV = Encoding.UTF8.GetBytes(encryptionModel.IV);

            ICryptoTransform decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

            using (MemoryStream ms = new MemoryStream(buffer))
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
