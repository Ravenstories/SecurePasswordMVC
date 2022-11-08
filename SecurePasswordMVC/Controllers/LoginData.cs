using Microsoft.AspNetCore.Mvc;

namespace SecurePasswordMVC.Controllers
{
    public class LoginData : Controller
    {
       
        public string Index()
        {
            //When user tries to login first check of many times the user have tried to login unsuccesfully before. 
            
            return "This is my default action...";
        }
       
        public string Welcome()
        {
            return "This is the Welcome action method...";
        }

    }
}
