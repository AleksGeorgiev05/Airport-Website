using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PlaneProject.Models;
using System.Web.Security;
using System.Text;
using System.Threading.Tasks;

namespace PlaneProject.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(UserModel model)
        {
            model.Role = "User";
            model.Password = Encode(model.Password);
            Procedures.Procedures.InsertUser(model);
            FormsAuthentication.SetAuthCookie(model.Username, false);
            return RedirectToAction("Index", "Home");
        }
        string Encode(string password)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserTable model, string returnUrl)
        {
            AirportDatabaseEntities db = new AirportDatabaseEntities();
            model.Password = Encode(model.Password);
            var dataItem = db.UserTable.Where(x => x.Username == model.Username && x.Password == model.Password).FirstOrDefault();
            if (dataItem != null)
            {
                FormsAuthentication.SetAuthCookie(dataItem.Username, false);
                if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                         && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Invalid Username or Password");
                return View();
            }
        }
    }
}