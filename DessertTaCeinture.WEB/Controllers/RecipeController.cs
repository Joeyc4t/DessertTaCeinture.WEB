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

        public ActionResult Create()
        {
            if (IsConnectedUser())
            {
                CreateRecipeModel model = new CreateRecipeModel()
                {
                    Categories = RecipeService.GetCategories(),
                    Origins = RecipeService.GetOrigins(),
                    Themes = RecipeService.GetThemes(),
                    RecipeIngredients = new List<Recipe_IngredientViewModel>()
                };

                model.RecipeIngredients.Add(InitIngredientRow());

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


                return RedirectToAction("Index");
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

        public ActionResult GetIngredientRow()
        {
            return PartialView("_RecipeIngredientRow", InitIngredientRow());
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

        private Recipe_IngredientViewModel InitIngredientRow()
        {
            Recipe_IngredientViewModel model = new Recipe_IngredientViewModel()
            {
                Ingredients = RecipeService.GetIngredients()
            };

            return model;
        }
        #endregion
    }
}
