using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class RecipeController : Controller
    {
        #region Instances
        public List<Recipe_IngredientModel> ingredients = new List<Recipe_IngredientModel>();
        private Recipe RecipeService = Services.Recipe.Instance;
        private Session SessionService = Services.Session.Instance;
        private User UserService = Services.User.Instance;

        #endregion Instances

        [HttpGet]
        public ActionResult Create()
        {
            if (IsConnectedUser())
            {
                RecipeViewModel viewModel = new RecipeViewModel()
                {
                    Categories = RecipeService.GetCategories(),
                    Origins = RecipeService.GetOrigins(),
                    Themes = RecipeService.GetThemes()
                };
                return View(viewModel);
            }
            else return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection, HttpPostedFileBase fileUpload)
        {
            CreateRecipeModel model = RecipeService.MapCollectionToRecipeModel(Request, collection, ((UserModel)Session["loggedUser"]).Id);

            try
            {
                if (!ModelState.IsValid) return View(model);

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    model.Picture = "/Content/images/recipes/" + fileUpload.FileName;
                    fileUpload.SaveAs(Server.MapPath("~/Content/images/recipes/" + fileUpload.FileName));
                }
                else model.Picture = "/Content/images/news/default-image.png";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    // Create recipe
                    RecipeModel recipeModel = AutoMapper<CreateRecipeModel, RecipeModel>.AutoMap(model);
                    StringContent recipeInsert = new StringContent(JsonConvert.SerializeObject(recipeModel));
                    recipeInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
                    HttpResponseMessage recipeRes = await client.PostAsync("api/Recipe", recipeInsert);
                    // Get inserted id
                    int recipeId = 0;
                    Int32.TryParse(recipeRes.Headers.Location.Query.Split('=')[1], out recipeId);
                    if (recipeRes.IsSuccessStatusCode && recipeId > 0)
                    {
                        bool ingredientsComplete = await RecipeService.RegisterIngredientsLinks(client, recipeId, model.RecipeIngredients);
                        bool stepsComplete = await RecipeService.RegisterStepsLinks(client, recipeId, model.RecipeSteps);

                        if (ingredientsComplete && stepsComplete) return RedirectToAction("Index");
                        else return RedirectToAction("Error", "Home");
                    }
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Delete(int id)
        {
            if (IsConnectedUser())
            {
                RecipeModel model = RecipeService.GetRecipe(id);
                return View(model);
            }
            else return RedirectToAction("Error", "Home");
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

                    bool ingredientsComplete = await RecipeService.DeleteIngredientsLinks(client, id);
                    bool stepsComplete = await RecipeService.DeleteStepsLinks(client, id);

                    if (ingredientsComplete && stepsComplete)
                    {
                        if (await RecipeService.DeleteRecipe(client, id)) return RedirectToAction("Index");
                        else return RedirectToAction("Error", "Home");
                    }
                    else return RedirectToAction("Error", "Home");
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            RecipeModel item = RecipeService.GetRecipe(id);

            if (item != null)
            {
                RecipeDetailViewModel viewModel = new RecipeDetailViewModel()
                {
                    Title = item.Title,
                    CreationDate = item.CreationDate,
                    Picture = item.Picture,
                    Creator = UserService.GetUserById(item.CreatorId),
                    Category = RecipeService.GetCategory(item.CategoryId),
                    Origin = RecipeService.GetOrigin(item.OriginId),
                    Theme = RecipeService.GetTheme(item.ThemeId),
                    RecipeIngredients = RecipeService.GetLinkedIngredients(id),
                    RecipeSteps = RecipeService.GetLinkedSteps(id)
                };
                return View(viewModel);
            }
            else return RedirectToAction("Error", "Home");
        }

        public ActionResult Edit(int id)
        {
            if (IsConnectedUser())
            {
                RecipeViewModel model = RecipeService.GetRecipeFull(id);
                return View(model);
            } 
            else return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, RecipeViewModel recipeViewModel, HttpPostedFileBase fileUpload)
        {
            RecipeModel recipeModel = AutoMapper<RecipeViewModel, RecipeModel>.AutoMap(recipeViewModel) ;

            bool recipeUpdated;
            bool ingredientsUpdated;
            bool stepsUpdated;

            try
            {
                if (!ModelState.IsValid) return View(recipeViewModel);

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    recipeModel.Picture = "/Content/images/recipes/" + fileUpload.FileName;
                    fileUpload.SaveAs(Server.MapPath("~/Content/images/recipes/" + fileUpload.FileName));
                }

                using (var client = new HttpClient())
                {
                    recipeUpdated = await RecipeService.UpdateRecipe(client, recipeModel);
                    ingredientsUpdated = await RecipeService.UpdateIngredientsLinks(client, recipeViewModel.RecipeIngredients);
                    stepsUpdated = await RecipeService.UpdateStepsLinks(client, recipeViewModel.RecipeSteps);
                }

                if(recipeUpdated && ingredientsUpdated && stepsUpdated) return RedirectToAction("Index");
                else return RedirectToAction("Error", "Home");
            }
            catch
            {
                return View(recipeViewModel);
            }
        }

        public ActionResult Index()
        {
            if (IsConnectedUser()) return View(RecipeService.GetUserRecipes());
            else return RedirectToAction("Error", "Home");
        }

        #region Private methods
        private bool IsConnectedUser()
        {
            if (SessionService.GetConnectedUser() != null) return true;
            else return false;
        }

        #endregion Private methods
    }
}