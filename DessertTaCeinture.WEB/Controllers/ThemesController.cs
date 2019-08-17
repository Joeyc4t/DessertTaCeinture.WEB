using DessertTaCeinture.WEB.Models.Theme;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;

using Newtonsoft.Json;

using System;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace DessertTaCeinture.WEB.Controllers
{
    public class ThemesController : Controller
    {
        #region Instances
        private Session SessionService = Services.Session.Instance;
        private Logs logsService = Logs.Instance;
        private Themes themeService = Themes.Instance;
        #endregion Instances

        public ActionResult Index()
        {
            try
            {
                if (IsConnectedAdmin()) return View(themeService.GetAll());
                else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Theme/Index");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Create()
        {
            try
            {
                if (IsConnectedAdmin()) return View();
                else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Theme/Create - Get");
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ThemeModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(StaticValues.BASE_URI);

                        StringContent toInsert = new StringContent(JsonConvert.SerializeObject(model));
                        toInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

                        HttpResponseMessage Res = await client.PostAsync("api/Theme", toInsert);

                        if (Res.IsSuccessStatusCode) return RedirectToAction("Index");
                        else return RedirectToAction("Error", "Home");
                    }
                }
                else return View(model);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Themes/Create - Post");
                return RedirectToAction("Error", "Home");
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                if (IsConnectedAdmin()) return View(themeService.Get(id));
                else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Theme/Delete - Get");
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);

                    HttpResponseMessage Res = await client.DeleteAsync($"api/Theme?id={id}");

                    if (Res.IsSuccessStatusCode) return RedirectToAction("Index");
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Theme/Delete - Post");
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
