using Microsoft.AspNetCore.SignalR;
using SignalRClient.RSA;
using System.Security.Cryptography;

namespace SignalRClient.Hubs
{
    public class EncryptConnectionHub : Hub
    {
        Aes? aes;
        public Task EncryptConnection(RSAParameters publicKey)
        {
            Console.WriteLine("Invoking: EncryptConnection");

            aes = Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();

            Console.WriteLine(aes.Key);
            
            byte[] aesKey = RSA_EncryptDecrypt.RSAEncrypt(aes.Key, publicKey, true);
            byte[] aesIV = RSA_EncryptDecrypt.RSAEncrypt(aes.IV, publicKey, true);

            return Clients.Caller.SendAsync("EncryptReply", aesKey, aesIV );
        }
    }
}
