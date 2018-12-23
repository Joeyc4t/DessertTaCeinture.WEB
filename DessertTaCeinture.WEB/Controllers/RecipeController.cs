using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Enumerations;
using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Models.Theme;
using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace DessertTaCeinture.WEB.Controllers
{
    public class RecipeController : Controller
    {
        #region Instances
        private Session CurrentSession = Services.Session.Instance;
        #endregion

        public ActionResult Index()
        {            
            if(IsConnectedUser()) return View(GetUserRecipes());
            else return RedirectToAction("Error", "Home");
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            ViewBag.Ingredient = new SelectList(GetIngredients(), "Id", "Name");
            ViewBag.Unit = new SelectList(GetUnits());

            ViewBag.Origin = new SelectList(GetOrigins(), "Id", "Country");
            ViewBag.Category = new SelectList(GetCategories(), "Id", "Name");
            ViewBag.Theme = new SelectList(GetThemes(), "Id", "Name");

            return View();
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
            return View();
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
            return View();
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

        #region Private methods
        private bool IsConnectedUser()
        {
            if (CurrentSession.GetConnectedUser() != null) return true;
            else return false;
        }

        private List<RecipeModel> GetUserRecipes()
        {
            UserModel connectedUser = CurrentSession.GetConnectedUser();
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
            catch(Exception ex)
            {
                return null;
            }
        }

        private List<CategoryModel> GetCategories()
        {
            List<CategoryModel> items = new List<CategoryModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/Category").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (CategoryModel category in JsonConvert.DeserializeObject<List<CategoryModel>>(result))
                            {
                                items.Add(category);
                            }
                        }
                        else return null;
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<ThemeModel> GetThemes()
        {
            List<ThemeModel> items = new List<ThemeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/Theme").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (ThemeModel theme in JsonConvert.DeserializeObject<List<ThemeModel>>(result))
                            {
                                items.Add(theme);
                            }
                        }
                        else return null;
                    }
                }
                return items;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<OriginModel> GetOrigins()
        {
            List<OriginModel> items = new List<OriginModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/Origin").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (OriginModel origin in JsonConvert.DeserializeObject<List<OriginModel>>(result))
                            {
                                items.Add(origin);
                            }
                        }
                        else return null;
                    }
                }
                return items.OrderBy(i => i.Country).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<IngredientModel> GetIngredients()
        {
            List<IngredientModel> ingredients = new List<IngredientModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/Ingredient").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (IngredientModel category in JsonConvert.DeserializeObject<List<IngredientModel>>(result))
                            {
                                ingredients.Add(category);
                            }
                        }
                        else return null;
                    }
                }
                return ingredients.OrderBy(i => i.Name).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<string> GetUnits()
        {
            List<string> units = new List<string>();

            foreach (object item in Enum.GetValues(typeof(EIngredientUnits)))
            {
                units.Add(item.ToString());
            }
            return units;
        }
        #endregion
    }
}
