﻿using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;

using Newtonsoft.Json;

using System;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;

namespace DessertTaCeinture.WEB.Controllers
{
    public class RecipeController : Controller
    {
        #region Instances
        public List<Recipe_IngredientModel> ingredients = new List<Recipe_IngredientModel>();
        private Recipe recipeService = Services.Recipe.Instance;
        private Session sessionService = Services.Session.Instance;
        private User userService = Services.User.Instance;
        private Search searchService = Services.Search.Instance;
        #endregion Instances

        [HttpGet]
        public ActionResult Create()
        {
            if (IsConnectedUser())
            {
                RecipeViewModel viewModel = new RecipeViewModel()
                {
                    Categories = recipeService.GetCategories(),
                    Origins = recipeService.GetOrigins(),
                    Themes = recipeService.GetThemes()
                };
                return View(viewModel);
            }
            else return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<ActionResult> Create(FormCollection collection, HttpPostedFileBase fileUpload)
        {
            CreateRecipeModel model = recipeService.MapCollectionToRecipeModel(Request, collection, ((UserModel)Session["loggedUser"]).Id);

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
                        bool ingredientsComplete = await recipeService.RegisterIngredientsLinks(client, recipeId, model.RecipeIngredients);
                        bool stepsComplete = await recipeService.RegisterStepsLinks(client, recipeId, model.RecipeSteps);

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
                RecipeModel model = recipeService.GetRecipe(id);
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

                    bool ingredientsComplete = await recipeService.DeleteIngredientsLinks(client, id);
                    bool stepsComplete = await recipeService.DeleteStepsLinks(client, id);

                    if (ingredientsComplete && stepsComplete)
                    {
                        if (await recipeService.DeleteRecipe(client, id)) return RedirectToAction("Index");
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
        public ActionResult RecipesICanDo(string a, string b, string c, string d)
        {
            List<RecipeViewModel> recipes = new List<RecipeViewModel>();
            List<int> ingredientIds = new List<int>();

            int? ia = !string.IsNullOrEmpty(a) ? Convert.ToInt32(a) : (int?)null;
            int? ib = !string.IsNullOrEmpty(b) ? Convert.ToInt32(b) : (int?)null;
            int? ic = !string.IsNullOrEmpty(c) ? Convert.ToInt32(c) : (int?)null;
            int? id = !string.IsNullOrEmpty(d) ? Convert.ToInt32(d) : (int?)null;

            List<Recipe_IngredientModel> links = recipeService.GetRecipe_Ingredients(ia, ib, ic, id, true);

            if (ia.HasValue) ingredientIds.Add(ia.Value);
            if (ib.HasValue) ingredientIds.Add(ib.Value);
            if (ic.HasValue) ingredientIds.Add(ic.Value);
            if (id.HasValue) ingredientIds.Add(id.Value);

            foreach(int recipeId in links.Select(l => l.RecipeId).Distinct())
            {
                RecipeViewModel tmp = recipeService.GetRecipeFull(recipeId);
                if(tmp.RecipeIngredients.Select(ri => ri.IngredientId).Any(ingredientIds.Contains) && tmp.IsPublic)
                {
                    recipes.Add(tmp);
                }
            }

            return View(recipes);
        }

        [HttpGet]
        public ActionResult RecipesWithout(string a, string b, string c, string d)
        {
            List<RecipeViewModel> recipes = new List<RecipeViewModel>();
            List<int> ingredientIds = new List<int>();

            int? ia = !string.IsNullOrEmpty(a) ? Convert.ToInt32(a) : (int?)null;
            int? ib = !string.IsNullOrEmpty(b) ? Convert.ToInt32(b) : (int?)null;
            int? ic = !string.IsNullOrEmpty(c) ? Convert.ToInt32(c) : (int?)null;
            int? id = !string.IsNullOrEmpty(d) ? Convert.ToInt32(d) : (int?)null;

            List<Recipe_IngredientModel> links = recipeService.GetRecipe_Ingredients(ia, ib, ic, id, false);

            if (ia.HasValue) ingredientIds.Add(ia.Value);
            if (ib.HasValue) ingredientIds.Add(ib.Value);
            if (ic.HasValue) ingredientIds.Add(ic.Value);
            if (id.HasValue) ingredientIds.Add(id.Value);

            foreach (int recipeId in links.Select(l => l.RecipeId).Distinct())
            {
                RecipeViewModel tmp = recipeService.GetRecipeFull(recipeId);
                if (!tmp.RecipeIngredients.Select(ri => ri.IngredientId).Any(ingredientIds.Contains) && tmp.IsPublic)
                {
                    recipes.Add(tmp);
                }
            }

            return View(recipes);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            RecipeModel item = recipeService.GetRecipe(id);

            if (item != null)
            {
                RecipeDetailViewModel viewModel = new RecipeDetailViewModel()
                {
                    Id = item.Id,
                    Title = item.Title,
                    CreationDate = item.CreationDate,
                    Picture = item.Picture,
                    Creator = userService.GetUserById(item.CreatorId),
                    Category = recipeService.GetCategory(item.CategoryId),
                    Origin = recipeService.GetOrigin(item.OriginId),
                    Theme = recipeService.GetTheme(item.ThemeId),
                    RecipeIngredients = recipeService.GetLinkedIngredients(id),
                    RecipeSteps = recipeService.GetLinkedSteps(id)
                };
                return View(viewModel);
            }
            else return RedirectToAction("Error", "Home");
        }

        public ActionResult Edit(int id)
        {
            if (IsConnectedUser())
            {
                RecipeViewModel model = recipeService.GetRecipeFull(id);
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
                    recipeUpdated = await recipeService.UpdateRecipe(client, recipeModel);
                    ingredientsUpdated = await recipeService.UpdateIngredientsLinks(client, recipeViewModel.RecipeIngredients);
                    stepsUpdated = await recipeService.UpdateStepsLinks(client, recipeViewModel.RecipeSteps);
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
            if (IsConnectedUser()) return View(recipeService.GetUserRecipes());
            else return RedirectToAction("Error", "Home");
        }

        #region Private methods
        private bool IsConnectedUser()
        {
            if (sessionService.GetConnectedUser() != null) return true;
            else return false;
        }

        #endregion Private methods
    }
}