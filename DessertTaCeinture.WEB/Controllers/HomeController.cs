using DessertTaCeinture.WEB.Models.Home;
using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class HomeController : Controller
    {
        #region Instances
        private News NewsService = Services.News.Instance;
        private Recipe RecipeService = Recipe.Instance;

        #endregion Instances

        public ActionResult Error()
        {
            ViewBag.Title = "Une erreur est survenue";
            return View("~/Views/Shared/Errors/GenericError.cshtml");
        }
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.News = NewsService.GetLastPublished();

            return View(model);
        }
        public ActionResult News()
        {
            return View(NewsService.GetAll());
        }
        public ActionResult NotAuthorized()
        {
            ViewBag.Title = "Accès non autorisé";
            return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
        }
        public ActionResult RandomRecipe()
        {
            Random random = new Random();
            int[] indexes = RecipeService.GetRecipeIndexes();
            int i = random.Next(0, indexes.Count());
            return RedirectToAction("Details", "Recipe", new { id = indexes[i] });
        }
        public ActionResult Recipes()
        {
            List<RecipeModel> model = RecipeService.GetLastPublished().ToList();
            return View(model);
        }
        public ActionResult TopRecipes()
        {
            return View();
        }
    }
}