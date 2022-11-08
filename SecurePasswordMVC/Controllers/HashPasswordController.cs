using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace SecurePasswordMVC.Controllers
{
    public class HashPasswordController : Controller
    {
        public static byte[] GenerateSalt()
        {
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            var randomNumber = new byte[32];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        public static byte[] Sha512(byte[] textToHash, byte[]? key)
        {
            if (key != null)
            {
                using var hmac = new HMACSHA512(key);
                return hmac.ComputeHash(textToHash);
            }
            else
            {
                using var sha512 = SHA512.Create();
                return sha512.ComputeHash(textToHash);
            }

        }
    
    }
}
