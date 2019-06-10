using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Services;

using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class Recipe_IngredientController : Controller
    {
        #region Instances
        private Recipe recipeService = Recipe.Instance;
        #endregion

        public ActionResult CreateField(CreateRecipeModel model, int? index)
        {
            ViewBag.Index = index ?? 0;
            return PartialView(model);
        }

        public ActionResult EditField(int recipeId)
        {
            RecipeViewModel viewModel = recipeService.GetRecipeFull(recipeId);
            return PartialView(viewModel);
        }
    }
}