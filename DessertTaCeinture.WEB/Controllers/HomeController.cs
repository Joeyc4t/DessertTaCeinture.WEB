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
        private Logs logsService = Logs.Instance;
        private Session SessionService = Services.Session.Instance;
        private Rate rateService = Rate.Instance;
        #endregion Instances

        public ActionResult Error()
        {
            try
            {
                ViewBag.Title = "Une erreur est survenue";
                return View("~/Views/Shared/Errors/GenericError.cshtml");
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/Error");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult Index()
        {
            try
            {
                IndexViewModel model = new IndexViewModel();
                model.News = newsService.GetLastPublished();

                return View(model);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/Index");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult News()
        {
            try
            {
                return View(newsService.GetAll());
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/News");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult NotAuthorized()
        {
            try
            {
                ViewBag.Title = "Accès non autorisé";
                return View("~/Views/Shared/Errors/NotAuthorized.cshtml");
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/NotAuthorized");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult RandomRecipe()
        {
            try
            {
                Random random = new Random();
                int[] indexes = recipeService.GetRecipeIndexes();
                int i = random.Next(0, indexes.Count());

                int recipeId = indexes[i];

                return View(recipeId);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/RandomRecipe");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult Recipes()
        {
            try
            {
                List<RecipeModel> model = recipeService.GetLastPublished().ToList();
                return View(model);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/Recipes");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult TopRecipes()
        {
            try
            {
                IEnumerable<RecipeModel> models = recipeService.GetTopRecipes();

                foreach(RecipeModel item in models)
                {
                    item.Average = rateService.CalculateAverage(item.Id);
                }

                return View(models);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/TopRecipes");
                return RedirectToAction("Error", "Home");
            }
        }
        public ActionResult QuickSearch(string searchtext)
        {
            try
            {
                var models = recipeService.GetPublicRecipes();
                if (!string.IsNullOrEmpty(searchtext))
                {
                    searchtext = searchService.ConvertSearchString(searchtext);
                    var result = new List<RecipeModel>();
                    foreach (RecipeModel model in models)
                    {
                        if (searchService.ConvertSearchString(model.Title).Contains(searchtext)) result.Add(model);
                    }
                    return View(result);
                }
                return View(models);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Home/QuickSearch");
                return RedirectToAction("Error", "Home");
            }            
        }       
    }
}