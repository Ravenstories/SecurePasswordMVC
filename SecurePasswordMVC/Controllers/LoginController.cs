using Microsoft.AspNetCore.Mvc;
using SecurePasswordMVC.Data;
using SecurePasswordMVC.Models;
using System.Security.Cryptography;
using System.Text;

namespace SecurePasswordMVC.Controllers
{
    public class LoginController : Controller
    {
        private readonly DB_DataContext _db;

        public LoginController(DB_DataContext db)
        {
            _db = db;
        }

        public IActionResult Login(User user)
        {
            //I want to add more checks, fx if two attemps were made under a secund there might be cause for concern.
            if (user.Username != null && LoginCounter.Counter <= 5)
            {
                var dbUser = _db.Users.Where(m => m.Username == user.Username).FirstOrDefault();

                if (dbUser != null)
                {
                    var salt = dbUser.Salt;

                    user.Password = Convert.ToBase64String(HashPasswordController.Sha512(Encoding.UTF8.GetBytes(user.Password), null));
                    var userPassword = Convert.ToBase64String(HashPasswordController.Sha512(Encoding.UTF8.GetBytes(user.Password+salt), null));
                    
                    if (userPassword == dbUser.Password)
                    {
                        Console.WriteLine("Login succesful");
                        UpdateUser(dbUser);
                        return RedirectToAction("LoginSuccess");
                    }
                    else
                    {
                        LoginCounter.Counter++;
                        Console.WriteLine("Failed attempts " + LoginCounter.Counter);
                    }
                }

            }else if(LoginCounter.Counter >= 5)
            {
                return RedirectToAction("LoginBlocked");
            }
            
            return View();
        }
        public IActionResult LoginSuccess()
        {
            return View();
        }

        public IActionResult LoginSeeDB()
        {
            IEnumerable<User> dbUsers = _db.Users;
            return View(dbUsers);
        }

        public IActionResult LoginBlocked()
        {
            Console.WriteLine("Failed attempts" + LoginCounter.Counter);
            return RedirectToAction("Home", "Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public void UpdateUser(User user)
        {
            var saltbyte = HashPasswordController.GenerateSalt();
            user.Salt = Convert.ToBase64String(saltbyte);
            user.Password = Convert.ToBase64String(HashPasswordController.Sha512(Encoding.UTF8.GetBytes(user.Password+ user.Salt), null));
            user.LastLogin = DateTime.Now;

            _db.Users.Update(user);

            _db.SaveChanges();

        }

    }
}
