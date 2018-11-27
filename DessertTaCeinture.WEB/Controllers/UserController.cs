using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
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
                    }
                    return RedirectToAction("Index", "Home");
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
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!AuthService.LoginUser(model))
            {
                return View(model);
            }            
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

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPut]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpDelete]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
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
