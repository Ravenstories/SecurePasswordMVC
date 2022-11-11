using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecurePasswordMVC.Data;
using SecurePasswordMVC.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SecurePasswordMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly DB_DataContext _db;
        public LoginController(DB_DataContext db)
        {
            //A database connection in a neat little object we can use. 
            _db = db;
        }
        
        /// <summary>
        /// Login rutine that checks if the user had made several attemps. 
        /// It uses a hashing function hide our data and the real password is never shown. 
        /// </summary>
        /// <param name="user">We recieve a user object from view, we can use to compare with</param>
        /// <returns>Sends a new user object to DB with an updated salt</returns>
        public IActionResult Login(User user)
        {
            //I want to add more checks, fx if two attemps were made under a second there might be cause for concern.
            if (user.Username != null && LoginCounter.Counter + 1 <= LoginCounter.MaxTries)
            {
                var alldbuser = _db.Users;
                var dbUser = _db.Users.Where(m => m.Username == user.Username).FirstOrDefault();
                var userPassword = user.Password;

                if (dbUser != null)
                {
                    byte[] encodedPassword = ASCIIEncoding.ASCII.GetBytes(user.Password);

                    //If the salt is null, it's the first time the user logs in. 
                    if (dbUser.Salt != null)
                    {
                        byte[] passwordToHash = Combine(encodedPassword, dbUser?.Salt);
                        userPassword = Convert.ToBase64String(HashPasswordController.Sha512(passwordToHash, null));
                    }
                
                    if (userPassword == dbUser.Password)
                    {
                        Console.WriteLine("Login succesful");
                        dbUser.Password = user.Password;
                        UpdateUser(dbUser);
                        LoginCounter.Counter = 0;
                        return RedirectToAction("LoginSuccess");
                    }
                    else
                    {
                        LoginCounter.Counter++;
                        Console.WriteLine("Failed attempts " + LoginCounter.Counter);
                        return View();
                    }
                }

            }else if(LoginCounter.Counter >= LoginCounter.MaxTries)
            {
                return RedirectToAction("LoginBlocked");
            }
            
            return View();
        }
        public IActionResult LoginSuccess()
        {
            return View();
        }

        //This page is a test page to show what's in the database. 
        public IActionResult LoginSeeDB()
        {
            IEnumerable<User> dbUsers = _db.Users;
            return View(dbUsers);
        }

        //If the user tries enough times, it will close for this session. 
        public IActionResult LoginBlocked()
        {
            Console.WriteLine("Failed attempts" + LoginCounter.Counter);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //When the user has succesfully logged in, we want to update the user info so it wont stay the same. 
        //As to avoid the data being cought and bruteforced. 
        public void UpdateUser(User user)
        {
            byte[] saltbyte = HashPasswordController.GenerateSalt();
            user.Salt = saltbyte;

            var encodedPassword = ASCIIEncoding.ASCII.GetBytes(user.Password);
            byte[] passwordToHash = Combine(encodedPassword, saltbyte);

            user.Password = Convert.ToBase64String(HashPasswordController.Sha512(passwordToHash, null));
            user.LastLogin = DateTime.Now;

            _db.Users.Update(user);

            _db.SaveChanges();

        }

        //Helper function to ad Salt to the user password for hashing. 
        public byte[] Combine(byte[] first, byte[]? second)
        {
            if (second != null)
            return first.Concat(second).ToArray();
            else
                return first;
        }


    }
}
