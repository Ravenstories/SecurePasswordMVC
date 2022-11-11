using Microsoft.AspNetCore.SignalR;
using SignalRClient.RSA;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace SignalRClient.Hubs
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            // Encrypt message with AES key. 

            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }

        Aes? aes;
        public Task Encrypt()
        {
            Console.WriteLine("Server: Please Encrypt");
            return Clients.All.SendAsync("EncryptReply", "Please Encrypt");
        }

        public Task Encrypt_Connection(string string_publicKey)
        {
            // Recieve pupkey, encrypt AES and return
            Console.WriteLine("Invoking: EncryptConnection with publickey: " + string_publicKey);

            //var byte_publicKey = Encoding.UTF8.GetBytes(string_publicKey);

            aes = Aes.Create();
            aes.GenerateIV();
            aes.GenerateKey();

            Console.WriteLine(aes.Key);
            RSACryptoServiceProvider rsa = new();

            // The method you use from the rsa object will vary depending on the encoding of the key
            //rsa.ImportSubjectPublicKeyInfo(byte_publicKey, out _);
            
            rsa.FromXmlString(string_publicKey);
            
            RSAParameters RSAParameters_publicKey = rsa.ExportParameters(false);

            byte[] aesKey = RSA_EncryptDecrypt.RSAEncrypt(aes.Key, RSAParameters_publicKey, true);
            byte[] aesIV = RSA_EncryptDecrypt.RSAEncrypt(aes.IV, RSAParameters_publicKey, true);

            Clients.All.SendAsync("ReceiveMessage", "Server", "Sending Encrypted Sym Key");

            return Clients.Caller.SendAsync("Encrypt_EncSymKey", aesKey, aesIV);
        }
        
        
    }

}
