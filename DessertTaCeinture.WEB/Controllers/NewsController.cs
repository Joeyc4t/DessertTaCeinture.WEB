using DessertTaCeinture.WEB.Models.News;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class NewsController : Controller
    {
        #region Instances
        private News NewsService = News.Instance;
        private Session SessionService = Services.Session.Instance;

        #endregion Instances

        public ActionResult Create()
        {
            if (IsConnectedAdmin()) return View();
            else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(NewsModel model, HttpPostedFileBase fileUpload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        model.ImageUrl = "/Content/images/news/" + fileUpload.FileName;
                        fileUpload.SaveAs(Server.MapPath("~/Content/images/news/" + fileUpload.FileName));
                    }
                    else model.ImageUrl = "/Content/images/news/default-image.png";

                    if (model.ReleaseDate == null)
                        model.ReleaseDate = DateTime.Now;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(StaticValues.BASE_URI);

                        StringContent toInsert = new StringContent(JsonConvert.SerializeObject(model));
                        toInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage Res = await client.PostAsync("api/News", toInsert);

                        if (Res.IsSuccessStatusCode) return RedirectToAction("Index");
                        else return RedirectToAction("Error", "Home");
                    }
                }
                else return View(model);
            }
            catch
            {
                return View(model);
            }
        }
        public ActionResult Delete(int id)
        {
            if (IsConnectedAdmin()) return View(NewsService.Get(id));
            else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
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

                    HttpResponseMessage Res = await client.DeleteAsync($"api/News?id={id}");

                    if (Res.IsSuccessStatusCode) return RedirectToAction("Index");
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult Details(int id)
        {
            return View(NewsService.Get(id));
        }
        public ActionResult Edit(int id)
        {
            if (IsConnectedAdmin())
            {
                NewsModel model = NewsService.Get(id);
                return View(model);
            }
            else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id, NewsModel model, HttpPostedFileBase fileUpload)
        {
            if (id == model.Id)
            {
                NewsModel initialModel = NewsService.Get(id);

                try
                {
                    if (fileUpload != null && fileUpload.ContentLength > 0)
                    {
                        model.ImageUrl = "/Content/images/news/" + fileUpload.FileName;
                        fileUpload.SaveAs(Server.MapPath("~/Content/images/news/" + fileUpload.FileName));
                    }
                    else model.ImageUrl = initialModel.ImageUrl;

                    if (model.ReleaseDate == null || model.ReleaseDate == DateTime.MinValue)
                        model.ReleaseDate = initialModel.ReleaseDate;

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(StaticValues.BASE_URI);

                        StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(model));
                        toUpdate.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        HttpResponseMessage Res = await client.PutAsync($"api/News?id={id}", toUpdate);
                        if (Res.IsSuccessStatusCode) return RedirectToAction("Index");
                        else return RedirectToAction("Error", "Home");
                    }
                }
                catch
                {
                    return View();
                }
            }
            else return RedirectToAction("Error", "Home");
        }
        public ActionResult Index()
        {
            if (IsConnectedAdmin()) return View(NewsService.GetAll());
            else return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
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