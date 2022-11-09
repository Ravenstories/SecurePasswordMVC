using Microsoft.AspNetCore.Mvc;
using SecurePasswordMVC.Models;
using System.Security.Cryptography;

namespace SecurePasswordMVC.Controllers
{
    public class EncryptionController : Controller
    {
        public IActionResult Encryption()
        {
            var vm = new EncryptionModel();
          
            ViewBag.SavedText = EncryptionModel.savedText;
            ViewBag.Timed = EncryptionModel.timed;
            return View(vm);
        }
        public IActionResult EncryptDecrypt(EncryptionModel encryptionModel)
        {
            if (encryptionModel.PlainText == null && encryptionModel.CipherText == null)
            {
                return RedirectToAction("Encryption");
            }
            else
            {
                if (encryptionModel.UserChosenAlgorithm == "AES" || encryptionModel.UserChosenAlgorithm == null)
                {
                    encryptionModel.Algorithm = EncryptionModel.AES;
                }
                else
                {
                    encryptionModel.Algorithm = EncryptionModel.Triple_DES;
                }
                
                //Date time and date span to see how long the process takes. 
                DateTime timeStart = DateTime.Now;
                
                if (encryptionModel.PlainText != null)
                {
                    string returnedText = EncryptionModel.Encrypt(encryptionModel, encryptionModel.Algorithm, encryptionModel.PlainText);
                    EncryptionModel.savedText = returnedText;
                    Console.WriteLine(returnedText);
                }
                else if(encryptionModel.CipherText != null)
                {
                    string returnedText = EncryptionModel.Decrypt(encryptionModel, encryptionModel.Algorithm, encryptionModel.CipherText);
                    EncryptionModel.savedText = returnedText;
                    Console.WriteLine(returnedText);
                }

                DateTime timeEnd = DateTime.Now;
                EncryptionModel.timed = timeEnd.Subtract(timeStart);
                
                return RedirectToAction("Encryption");
            }
        }

        


    }
}
