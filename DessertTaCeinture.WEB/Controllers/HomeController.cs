using DessertTaCeinture.WEB.Models.Home;
using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Services;

using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Controllers
{
    public class HomeController : Controller
    {
        #region Instances
        private News newsService = Services.News.Instance;
        private Recipe recipeService = Recipe.Instance;
        private Search searchService = Search.Instance;
        #endregion Instances

        public ActionResult Error()
        {
            ViewBag.Title = "Une erreur est survenue";
            return View("~/Views/Shared/Errors/GenericError.cshtml");
        }
        public ActionResult Index()
        {
            IndexViewModel model = new IndexViewModel();
            model.News = newsService.GetLastPublished();

            return View(model);
        }
        public ActionResult News()
        {
            return View(newsService.GetAll());
        }
        public ActionResult NotAuthorized()
        {
            ViewBag.Title = "Accès non autorisé";
            return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
        }
        public ActionResult RandomRecipe()
        {
            Random random = new Random();
            int[] indexes = recipeService.GetRecipeIndexes();
            int i = random.Next(0, indexes.Count());

            int recipeId = indexes[i];

            return View(recipeId);
        }
        public ActionResult Recipes()
        {
            List<RecipeModel> model = recipeService.GetLastPublished().ToList();
            return View(model);
        }
        public ActionResult TopRecipes()
        {
            IEnumerable<RecipeModel> models = recipeService.GetTopRecipes();
            return View(models);
        }
        public ActionResult QuickSearch(string searchtext)
        {
            var models = recipeService.GetPublicRecipes();
            if (!string.IsNullOrEmpty(searchtext))
            {
                searchtext = searchService.ConvertSearchString(searchtext);
                var result = new List<RecipeModel>();
                foreach(RecipeModel model in models)
                {
                    if (searchService.ConvertSearchString(model.Title).Contains(searchtext)) result.Add(model);
                }
                return View(result);
            }
            return View(models);
        }       
    }
}