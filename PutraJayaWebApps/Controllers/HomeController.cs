using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace PutraJayaWebApps.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
         
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username,string password)
        {
            var db = new Entities.PutraJayaAppsEntities();
            var encrypt = MD5Encryption(password + ConfigurationManager.AppSettings["hashPassword"].ToString());
            var user = db.MsUsers.FirstOrDefault(x => x.UserName == username && x.Password == encrypt);
            if (user != null)
            {
                Session["username"] = user.UserName;
                return RedirectToAction("Index", "Dashboard");
            }
            return RedirectToAction("login","home", new { login="failed" });
        }
        public static string MD5Encryption(string encryptionText)
        {


            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(encryptionText));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}