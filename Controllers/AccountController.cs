using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Swertres.Web.Models.ViewModels;
using Swertres.Web.Models.DataManager;
using System.Web.Security;
using Swertres.Web.Models.Helpers;

namespace Swertres.Web.Controllers
{
    public class AccountController : Controller
    {
        AccountManager _am; 
        public AccountController()
        {
            _am = new AccountManager();
        }

        public ActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LogIn(UserLoginView dto, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string password = _am.GetUserPassword(dto.UserName);

                if (string.IsNullOrEmpty(password))
                    ModelState.AddModelError("", "The username or password provided is incorrect.");
                else
                {
                    if (Security.VerifySaltedHash(password,dto.Password))
                    {
                        FormsAuthentication.RedirectFromLoginPage(dto.UserName, true);
                    }
                    else
                    {
                        ModelState.AddModelError("", "The password provided is incorrect.");
                    }
                }
            }

            // If we got this far, something failed, redisplay form  
            return View(dto);
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}