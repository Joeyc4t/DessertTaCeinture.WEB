using DessertTaCeinture.WEB.Models.Rate;
using DessertTaCeinture.WEB.Models.Recipe;
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
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class UserController : Controller
    {
        #region Instances
        private Authentification AuthService = Services.Authentification.Instance;
        private User UserService = Services.User.Instance;
        private Logs logsService = Logs.Instance;
        private Session SessionService = Services.Session.Instance;
        private Recipe recipeService = Recipe.Instance;
        private Rate rateService = Rate.Instance;
        #endregion Instances

        public ActionResult Create()
        {
            if (!UserService.IsConnectedUser()) return View();
            else
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Utilisateur connecté", "User/Create - Get");
                return RedirectToAction("Error", "Home");
            } 
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (UserService.IsConnectedUser()) return View(UserService.GetAll());
            else
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Aucun utilisateur connecté", "User/Index - Get");
                return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
            }                
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
                        toInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

                        HttpResponseMessage Res = await client.PostAsync("api/User", toInsert);
                        if (Res.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
                        else return View(model);
                    }
                }
                catch(Exception ex)
                {
                    logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "User/Create - Post");
                    return RedirectToAction("Error", "Home");                    
                }
            }
            return View(model);
        }

        public ActionResult Activate(int? id)
        {
            if (id == null || !UserService.IsConnectedUser())
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Aucun utilisateur connecté", "User/Activate - Get");
                return RedirectToAction("NotAuthorized", "Home");
            }            

            UserModel model = UserService.GetConnectedUser();
            if (model.Id != id && UserService.IsConnectedAdmin()) return View("~/Views/Admin/ActivateUser.cshtml", model);
            else
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Id du model différent ou Admin non connecté", "User/Activate - Get");
                return RedirectToAction("Error", "Home");
            }               
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
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "User/Activate - Post");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Delete(int? id)
        {
            if (id == null || !UserService.IsConnectedUser())
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Aucun utilisateur connecté", "User/Delete - Get");
                return RedirectToAction("NotAuthorized", "Home");
            }

            UserModel model = UserService.GetConnectedUser();
            if (model.Id != id && UserService.IsConnectedAdmin()) return View("~/Views/Admin/DeleteUser.cshtml", model);
            else
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Id du model différent ou Admin non connecté", "User/Delete - Get");
                return RedirectToAction("Error", "Home");
            }
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
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "User/DeleteConfirmed - Post");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Details()
        {
            if (UserService.IsConnectedUser())
            {
                DetailsModel detailsModel = AutoMapper<UserModel, DetailsModel>.AutoMap(UserService.GetConnectedUser());
                var associatedRecipes = recipeService.GetUserRecipes();
                var associatedRates = rateService.GetRates().Where(r => r.UserId.Equals(detailsModel.Id));

                detailsModel.associatedRecipes = associatedRecipes != null ? associatedRecipes.ToList() : new List<RecipeModel>();
                detailsModel.associatedRates = associatedRates != null ? associatedRates.ToList() : new List<RateModel>();

                return View(detailsModel);
            }
            else
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Aucun utilisateur connecté", "User/Details - Get");
                return RedirectToAction("Error", "Home");
            }
            
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Id null", "User/Edit - Get");
                return RedirectToAction("Error", "Home");
            } 
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
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

                    HttpResponseMessage Res = await client.PutAsync($"api/User?id={localModel.Id}", toUpdate);
                    if (Res.IsSuccessStatusCode) return RedirectToAction("Index", "Home");
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "User/Edit - Post");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!UserService.IsConnectedUser()) return View();
            else
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, "Utilisateur déjà connecté", "User/Login - Get");
                return RedirectToAction("Error", "Home");
            } 
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