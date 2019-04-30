using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class AdminController : Controller
    {
        #region Instances
        private Authentification AuthService = Services.Authentification.Instance;
        private Session SessionService = Services.Session.Instance;

        #endregion Instances

        public ActionResult Index()
        {
            if (!IsConnectedAdmin()) return RedirectToAction("Login");
            else return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!IsConnectedAdmin()) return View();
            else return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (!AuthService.LoginUser(model)) return View(model);
            return RedirectToAction("Index", "Admin");
        }

        public ActionResult Logout()
        {
            AuthService.Logout();
            return RedirectToAction("Index", "Admin");
        }

        #region Private methods
        private bool IsConnectedAdmin()
        {
            if (SessionService.GetConnectedAdmin() != null) return true;
            else return false;
        }

        #endregion Private methods
    }
}