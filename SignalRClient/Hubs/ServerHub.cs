using Microsoft.AspNetCore.SignalR;
using SignalRClient.Functions;
using System.Collections;
using System.Security.Cryptography;
using System.Text;

namespace SignalRClient.Hubs
{
    /// <summary>
    /// This simulates the server our connections negotiates with. 
    /// </summary>
    public class ServerHub : Hub
    {
        // I was thinking about storing this in a session or something simular, but haven't set my mind on anything yet. 
        static SymmetricAlgorithm aes = Aes.Create();

        public async Task SendMessage(byte[] user, byte[] message)
        {
            // Decrypt message with AES key. 
            string decryptedUser = Aes_EncryptDecrypt.Decrypt(aes, user);
            string decryptedMessage = Aes_EncryptDecrypt.Decrypt(aes, message);
            
            //In this testcase scenario the server actually doesn't have to decrypt this message since it's send to all
            //But in other cases it might be valid so I show it here. 

            await Clients.All.SendAsync("ReceiveSecureMessage", user, message);
        }

        /// <summary>
        /// This little node tells the user to encrypt when connection is established between this hub and the client
        /// </summary>
        /// <returns></returns>

        public Task Encrypt()
        {
            Console.WriteLine("Server: Please Encrypt");
            return Clients.Caller.SendAsync("EncryptReply", "Please Encrypt");
        }

        /// <summary>
        /// The server tries to encrypt an AES Key and IV with the public key provided
        /// It will then save that AES key for future use with the client. 
        /// </summary>
        /// <returns>ENCRYPTED AES KEY AND IV</returns>

        public Task Encrypt_Connection(string string_publicKey)
        {
            try
            {
                // Recieve pupkey, encrypt AES and return

                aes.GenerateIV();
                aes.GenerateKey();

                //Creating and RSA object so we can use it to get the public key from the xml string
                RSACryptoServiceProvider rsa = new();
                rsa.FromXmlString(string_publicKey);
                RSAParameters RSAParameters_publicKey = rsa.ExportParameters(false);

                //SignalR allows us to send more than one object so we can encrypt it in two parts.
                byte[] aesKey = RSA_EncryptDecrypt.RSAEncrypt(aes.Key, RSAParameters_publicKey, true);
                byte[] aesIV = RSA_EncryptDecrypt.RSAEncrypt(aes.IV, RSAParameters_publicKey, true);

                //Just to visualize in view what happens. 
                Clients.Caller.SendAsync("ReceiveMessage", "Server", "Sending Encrypted Sym Key");

                return Clients.Caller.SendAsync("Encrypt_EncSymKey", aesKey, aesIV);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exeption: " + e);
                return Clients.Caller.SendAsync("RecieveMessage", "Encryption_Connection Failed");
            }
        }
        
        
    }

}
