using DessertTaCeinture.WEB.Models.News;
using DessertTaCeinture.WEB.Services;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class NewsController : Controller
    {
        #region Instances
        private News NewsService = News.Instance;
        #endregion

        public ActionResult Index()
        {
            return View(NewsService.GetAll());
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(NewsModel model)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
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

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
