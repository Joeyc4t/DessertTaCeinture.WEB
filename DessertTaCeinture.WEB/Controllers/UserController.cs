using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class UserController : Controller
    {
        #region Instances
        private Authentification AuthService = Services.Authentification.Instance;
        private User UserService = Services.User.Instance;
        #endregion Instances

        public ActionResult Create()
        {
            if (!UserService.IsConnectedUser()) return View();
            else return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (UserService.IsConnectedUser()) return View(UserService.GetAll());
            else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserModel localModel = AutoMapper<RegisterModel, UserModel>.AutoMap(model);
                    localModel.Salt = BCrypt.Net.BCrypt.GenerateSalt();
                    localModel.Password = BCrypt.Net.BCrypt.HashPassword(model.Password, localModel.Salt);
                    localModel.InscriptionDate = DateTime.Now;
                    localModel.IsActive = true;
                    localModel.RoleId = 1;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(StaticValues.BASE_URI);

                        StringContent toInsert = new StringContent(JsonConvert.SerializeObject(localModel));
                        toInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage Res = await client.PostAsync("api/User", toInsert);
                        if (Res.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
                        else return RedirectToAction("Error", "Home");
                    }
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);
        }

        public ActionResult Activate(int? id)
        {
            if (id == null || !UserService.IsConnectedUser()) return RedirectToAction("NotAuthorized", "Home");

            UserModel model = UserService.GetConnectedUser();
            if (model.Id != id && UserService.IsConnectedAdmin()) return View("~/Views/Admin/ActivateUser.cshtml", model);
            else return RedirectToAction("Error", "Home");
        }

        [HttpPost, ActionName("Activate")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ActivateConfirmed(string email)
        {
            try
            {
                UserModel model = UserService.Get(email);
                model.IsActive = true;

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(model));
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage Res = await client.PutAsync($"api/User?id={model.Id}", toUpdate);

                    if (Res.IsSuccessStatusCode && !UserService.IsConnectedAdmin())
                    {
                        Logout();
                        return RedirectToAction("Index", "Home");
                    }
                    else if (Res.IsSuccessStatusCode && UserService.IsConnectedAdmin())
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null || !UserService.IsConnectedUser()) return RedirectToAction("Error", "Home");

            UserModel model = UserService.GetConnectedUser();
            if (model.Id != id && UserService.IsConnectedAdmin()) return View("~/Views/Admin/DeleteUser.cshtml", model);
            if(model.Id == id) return View(model);
            else return RedirectToAction("Error", "Home");

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.DeleteAsync($"api/User?id={id}").Result;

                    if (Res.IsSuccessStatusCode && !UserService.IsConnectedAdmin())
                    {
                        Logout();
                        return RedirectToAction("Index", "Home");
                    }
                    else if(Res.IsSuccessStatusCode && UserService.IsConnectedAdmin())
                    {
                        return RedirectToAction("Index", "User");
                    }
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Details()
        {
            if (UserService.IsConnectedUser()) return View(AutoMapper<UserModel, DetailsModel>.AutoMap(UserService.GetConnectedUser()));
            else return RedirectToAction("Error", "Home");
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return RedirectToAction("Error", "Home");
            return View();
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPwdModel model)
        {
            if (model.NewPassword == model.OldPassword) ModelState.AddModelError("NewPassword", "Le nouveau mot de passe doit être différent de l'ancien !");

            if (!ModelState.IsValid) return View(model);

            UserModel localModel = UserService.GetConnectedUser();

            if (localModel == null) return RedirectToAction("Error", "Home");

            try
            {
                using (var client = new HttpClient())
                {
                    localModel.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, localModel.Salt);

                    client.BaseAddress = new Uri(StaticValues.BASE_URI);

                    StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(localModel));
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage Res = await client.PutAsync($"api/User?id={localModel.Id}", toUpdate);
                    if (Res.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!UserService.IsConnectedUser()) return View();
            else return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);
            if (!AuthService.LoginUser(model)) return View(model);

            return RedirectToAction("Index", "Home");
        }

        public PartialViewResult LoginForm()
        {
            return PartialView("_LoginForm", new LoginModel());
        }

        public ActionResult Logout()
        {
            AuthService.Logout();
            return RedirectToAction("Index", "Home");
        }
    }
}