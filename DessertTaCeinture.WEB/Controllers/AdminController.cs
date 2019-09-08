using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using System;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class AdminController : Controller
    {
        #region Instances
        private Authentification AuthService = Services.Authentification.Instance;
        private Session SessionService = Services.Session.Instance;
        private Logs logsService = Services.Logs.Instance;
        private Recipe recipeService = Recipe.Instance;
        #endregion Instances

        public ActionResult Index()
        {
            try
            {
                if (!IsConnectedAdmin()) return RedirectToAction("Login");
                else return View();
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Index - Get");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            try
            {
                if (!IsConnectedAdmin()) return View();
                else return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Login - Get");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            try
            {
                if (!ModelState.IsValid) return View(model);
                if (!AuthService.LoginUser(model)) return View(model);
                return RedirectToAction("Index", "Admin");
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Login - Post");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Logout()
        {
            try
            {
                AuthService.Logout();
                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Logout");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Users()
        {
            try
            {
                return RedirectToAction("Index", "User");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Users");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult WaitingRecipes()
        {
            try
            {
                return View(recipeService.GetWaitingRecipes());
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/WaitingRecipes");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult News()
        {
            try
            {
                return RedirectToAction("Index", "News");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/News");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Categories()
        {
            try
            {
                return RedirectToAction("Index", "Categories");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Categories");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Themes()
        {
            try
            {
                return RedirectToAction("Index", "Themes");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Themes");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Logs()
        {
            try
            {
                return RedirectToAction("Index", "Logs");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Admin/Logs");
                return RedirectToAction("Error", "Home");
            }
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