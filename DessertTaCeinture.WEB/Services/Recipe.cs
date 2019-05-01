using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Recipe;
using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
using DessertTaCeinture.WEB.Models.Step;
using DessertTaCeinture.WEB.Models.Theme;
using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Tools;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DessertTaCeinture.WEB.Services
{
    public class Recipe
    {
        #region Instances
        private static Recipe _Instance;
        private Session SessionService = Services.Session.Instance;
        private Recipe()
        {
        }

        public static Recipe Instance
        {
            get { return _Instance = _Instance ?? new Recipe(); }
        }

        #endregion Instances

        public List<CategoryModel> GetCategories()
        {
            List<CategoryModel> items = new List<CategoryModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
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
            catch
            {
                // LOG ERROR
                return null;
            }
        }

        public List<IngredientModel> GetIngredients()
        {
            List<IngredientModel> ingredients = new List<IngredientModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
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
            catch
            {
                // LOG ERROR
                return null;
            }
        }

        public List<OriginModel> GetOrigins()
        {
            List<OriginModel> items = new List<OriginModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
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
            catch
            {
                // LOG ERROR
                return null;
            }
        }

        public List<ThemeModel> GetThemes()
        {
            List<ThemeModel> items = new List<ThemeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
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
            catch
            {
                // LOG ERROR
                return null;
            }
        }

        public List<RecipeModel> GetUserRecipes()
        {
            UserModel connectedUser = SessionService.GetConnectedUser();
            List<RecipeModel> recipes = new List<RecipeModel>();
            DataWrapper<RecipeModel> wrapper = new DataWrapper<RecipeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
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
            catch
            {
                // LOG ERROR
                return null;
            }
        }

        public async Task<bool> RegisterIngredientsLinks(HttpClient client, int recipeId, IList<Recipe_IngredientModel> items)
        {
            bool isComplete = true;
            // Create ingredients links
            foreach (var item in items)
            {
                item.RecipeId = recipeId;

                StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(item));
                itemInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage itemRes = await client.PostAsync("api/Recipe_Ingredients", itemInsert);

                if (itemRes.IsSuccessStatusCode) continue;
                else isComplete = false;
            }
            return isComplete;
        }

        public async Task<bool> RegisterStepsLinks(HttpClient client, int recipeId, IList<StepModel> items)
        {
            bool isComplete = true;
            // Create steps links
            foreach (var item in items)
            {
                item.RecipeId = recipeId;

                StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(item));
                itemInsert.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage itemRes = await client.PostAsync("api/Step", itemInsert);

                if (itemRes.IsSuccessStatusCode) continue;
                else isComplete = false;
            }
            return isComplete;
        }
    }
}