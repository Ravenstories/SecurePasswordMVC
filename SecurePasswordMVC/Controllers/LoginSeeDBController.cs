using Microsoft.AspNetCore.Mvc;
using SecurePasswordMVC.Data;
using SecurePasswordMVC.Models;

namespace SecurePasswordMVC.Controllers
{
    public class LoginSeeDBController : Controller
    {
        private readonly DB_DataContext _db;

        public LoginSeeDBController(DB_DataContext db)
        {
            _db = db;
        }

        public IActionResult LoginSeeDB()
        {
            IEnumerable<User> dbUsers = _db.Users; 
            return View(dbUsers);
        }
    }
}
