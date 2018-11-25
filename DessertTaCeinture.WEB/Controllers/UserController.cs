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
        private Authentification AuthService = Authentification.Instance;

        public ActionResult Details(int id)
        {
            return View();
        }

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

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
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

        [HttpPost]
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
    }
}
