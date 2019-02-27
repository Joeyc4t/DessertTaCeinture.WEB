﻿using DessertTaCeinture.WEB.Models.Recipe;
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
using System.Web.Http.Results;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class RecipeController : Controller
    {
        #region Instances
        public List<Recipe_IngredientModel> ingredients = new List<Recipe_IngredientModel>();
        private Recipe RecipeService = Services.Recipe.Instance;
        private Session SessionService = Services.Session.Instance;

        #endregion Instances

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
        public async Task<ActionResult> Create(FormCollection collection, HttpPostedFileBase fileUpload)
        {
            CreateRecipeModel model = new CreateRecipeModel()
            {
                CreationDate = DateTime.Now,
                CreatorId = ((UserModel)Session["loggedUser"]).Id,
                CategoryId = int.Parse(Request.Form["CategoryId"]),
                OriginId = int.Parse(Request.Form["OriginId"]),
                ThemeId = int.Parse(Request.Form["ThemeId"]),
                Title = Request.Form["Title"],
                RecipeIngredients = new List<Recipe_IngredientModel>()
            };

            List<string> requestResult = new List<string>();
            int i = 1;

            while (collection["RecipeIngredients[" + i + "]"] != null)
            {
                requestResult.Add(collection["RecipeIngredients[" + i + "]"]);
                i++;
            }

            for (int j = 0; j < requestResult.Count; j++)
            {
                string[] ingredient = requestResult[j].Split(',');
                model.RecipeIngredients.Add(new Recipe_IngredientModel()
                {
                    Index = j,
                    IngredientId = int.Parse(ingredient[0]),
                    Quantity = int.Parse(ingredient[1]),
                    Unit = ingredient[2]
                });
            }

            try
            {
                if (!ModelState.IsValid)
                    return View(model);

                if (fileUpload != null && fileUpload.ContentLength > 0)
                {
                    model.Picture = "/Content/images/recipes/" + fileUpload.FileName;
                    fileUpload.SaveAs(Server.MapPath("~/Content/images/recipes/" + fileUpload.FileName));
                }
                else model.Picture = "/Content/images/news/default-image.png";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    // Create recipe
                    RecipeModel recipeModel = AutoMapper<CreateRecipeModel, RecipeModel>.AutoMap(model);
                    StringContent recipeInsert = new StringContent(JsonConvert.SerializeObject(recipeModel));
                    recipeInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    HttpResponseMessage recipeRes = await client.PostAsync("api/Recipe", recipeInsert);
                    if (recipeRes.IsSuccessStatusCode)
                    {
                        // Create links
                        foreach (var item in model.RecipeIngredients)
                        {
                            StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(item));
                            itemInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            HttpResponseMessage itemRes = await client.PostAsync("api/Recipe_Ingredients", itemInsert);
                            if (itemRes.IsSuccessStatusCode) continue;
                            else RedirectToAction("Error", "Home");
                        }
                        return RedirectToAction("Index");
                    }
                    else
                        return RedirectToAction("Error", "Home");
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
        public ActionResult Details(int id)
        {
            if (IsConnectedUser())
                return View();
            else
                return RedirectToAction("Error", "Home");
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
        public ActionResult Index()
        {
            if (IsConnectedUser())
                return View(GetUserRecipes());
            else
                return RedirectToAction("Error", "Home");
        }

        #region Private methods
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
                        if (wrapper.container.entities.Count > 0)
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
        private bool IsConnectedUser()
        {
            if (SessionService.GetConnectedUser() != null) return true;
            else return false;
        }

        #endregion Private methods
    }
}