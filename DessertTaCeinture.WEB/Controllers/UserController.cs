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
        private Authentification AuthService = Authentification.Instance;
        private Session CurrentSession = Services.Session.Instance;
        #endregion

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RegisterModel model)
        {
            if(ModelState.IsValid)
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
                        client.BaseAddress = new Uri("http://localhost:50140/");

                        StringContent toInsert = new StringContent(JsonConvert.SerializeObject(localModel));
                        toInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage Res = await client.PostAsync("api/User", toInsert);
                        if (Res.IsSuccessStatusCode)
                            return RedirectToAction("Index", "Home");
                        else
                            return RedirectToAction("Error", "Home");
                    }
                }
                catch
                {
                    return View(model);
                }
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (!AuthService.LoginUser(model))
                return View(model);

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

        public ActionResult Details()
        {
            return View(AutoMapper<UserModel, DetailsModel>.AutoMap(CurrentSession.GetConnectedUser()));
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return RedirectToAction("Error", "Home");

            return View();
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditPwdModel model)
        {
            if (model.NewPassword == model.OldPassword)
                ModelState.AddModelError("NewPassword", "Le nouveau mot de passe doit être différent de l'ancien !");

            if (!ModelState.IsValid)
                return View(model);

            UserModel localModel = CurrentSession.GetConnectedUser();

            if (localModel == null)
                return RedirectToAction("Error", "Home");

            try
            {
                using (var client = new HttpClient())
                {
                    localModel.Password = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, localModel.Salt);

                    client.BaseAddress = new Uri("http://localhost:50140/");

                    StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(localModel));
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    HttpResponseMessage Res = await client.PutAsync($"api/User?id={localModel.Id}", toUpdate);
                    if (Res.IsSuccessStatusCode)
                        return RedirectToAction("Index", "Home");
                    else
                        return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return RedirectToAction("Error", "Home");

            UserModel model = CurrentSession.GetConnectedUser();
            if (model.Id != id)
                return RedirectToAction("Error", "Home");

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.DeleteAsync($"api/User?id={id}").Result;

                    if (Res.IsSuccessStatusCode)
                    {
                        Logout();
                        return RedirectToAction("Index", "Home");
                    }
                    else
                        return RedirectToAction("Error", "Home");
                }                
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }

        #region Private methods
        private UserModel GetLoggedUser(LoginModel model)
        {
            UserModel globalModel = new UserModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/User?id={model.Email}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        globalModel = JsonConvert.DeserializeObject<UserModel>(result);
                    }
                }
                return globalModel;
            }
            catch
            {
                return null;
            }
        }
        #endregion
    }
}
