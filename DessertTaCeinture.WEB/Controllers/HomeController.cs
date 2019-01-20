using DessertTaCeinture.WEB.Models.Home;
using DessertTaCeinture.WEB.Services;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class HomeController : Controller
    {
        #region Instances
        private News NewsService = News.Instance;
        #endregion

        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.News = NewsService.GetLastPublished();

            return View(model);
        }

        public ActionResult Recipes()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Errors/GenericError.cshtml");
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}