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
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class RecipeController : Controller
    {
        #region Instances
        private Session SessionService = Services.Session.Instance;
        private Recipe RecipeService = Services.Recipe.Instance;

        public List<Recipe_IngredientModel> ingredients = new List<Recipe_IngredientModel>();
        #endregion

        public ActionResult Index()
        {            
            if(IsConnectedUser())
                return View(GetUserRecipes());
            else
                return RedirectToAction("Error", "Home");
        }

        public ActionResult Details(int id)
        {
            if (IsConnectedUser())
                return View();
            else
                return RedirectToAction("Error", "Home");
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (IsConnectedUser())
            {
                CreateRecipeModel model = new CreateRecipeModel()
                {
                    Categories = RecipeService.GetCategories(),
                    Origins = RecipeService.GetOrigins(),
                    Themes = RecipeService.GetThemes(),
                    RecipeIngredients = new List<Recipe_IngredientModel>()
                };

                model.RecipeIngredients.Add(new Recipe_IngredientModel() { Index = 1 });

                return View(model);
            }
            else
                return RedirectToAction("Error", "Home");            
        }

        [HttpPost]
        public ActionResult Create(CreateRecipeModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                return RedirectToAction("Create");
            }
            catch
            {
                return View(model);
            }
        }

        public ActionResult Edit(int id)
        {
            if (IsConnectedUser())
                return View();
            else
                return RedirectToAction("Error", "Home");
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
            if (IsConnectedUser())
                return View();
            else
                return RedirectToAction("Error", "Home");
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

        [HttpGet]
        public ActionResult CreateField(int? index)
        {
            ViewBag.Index = index ?? 0;
            return PartialView(new Recipe_IngredientModel() { Index = (int)index });
        }

        [HttpPost]
        public void CreateField(Recipe_IngredientModel model)
        {
            ingredients.Add(model);
        }

        #region Private methods
        private bool IsConnectedUser()
        {
            if (SessionService.GetConnectedUser() != null) return true;
            else return false;
        }

        private List<RecipeModel> GetUserRecipes()
        {
            UserModel connectedUser = SessionService.GetConnectedUser();
            List<RecipeModel> recipes = new List<RecipeModel>();
            DataWrapper<RecipeModel> wrapper = new DataWrapper<RecipeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/Recipe/GetUserRecipes?id={connectedUser.Id}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        wrapper = JsonConvert.DeserializeObject<DataWrapper<RecipeModel>>(result);
                        if (wrapper != null)
                        {
                            foreach (RecipeModel recipe in wrapper.container.entities)
                            {
                                recipes.Add(recipe);
                            }
                        }
                        else return null;
                    }
                }
                return recipes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
