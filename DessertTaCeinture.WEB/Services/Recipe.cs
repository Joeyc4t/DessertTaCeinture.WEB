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
using System.Web;
using System.Web.Mvc;

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

        public async Task<bool> DeleteIngredientsLinks(HttpClient client, int recipeId)
        {
            bool isComplete = true;
            var links = GetLinkedIngredients(recipeId);
            if (links != null)
            {
                foreach (var item in links)
                {
                    HttpResponseMessage itemRes = await client.DeleteAsync($"api/Recipe_Ingredients?id={item.LinkId}");

                    if (itemRes.IsSuccessStatusCode) continue;
                    else isComplete = false;
                }
            }
            return isComplete;
        }
        public async Task<bool> DeleteRecipe(HttpClient client, int recipeId)
        {
            HttpResponseMessage itemRes = await client.DeleteAsync($"api/Recipe?id={recipeId}");
            return itemRes.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteStepsLinks(HttpClient client, int recipeId)
        {
            bool isComplete = true;
            var links = GetLinkedSteps(recipeId);
            if (links != null)
            {
                foreach (var item in links)
                {
                    HttpResponseMessage itemRes = await client.DeleteAsync($"api/Step?id={item.Id}");

                    if (itemRes.IsSuccessStatusCode) continue;
                    else isComplete = false;
                }
            }
            return isComplete;
        }
        public IEnumerable<CategoryModel> GetCategories()
        {
            List<CategoryModel> items = new List<CategoryModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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
        public CategoryModel GetCategory(int categoryId)
        {
            CategoryModel item = new CategoryModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                HttpResponseMessage Res = client.GetAsync($"api/Category/{categoryId}").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    if (result != null) item = JsonConvert.DeserializeObject<CategoryModel>(result);
                    else return null;
                }
            }
            return item;
        }
        public IEnumerable<IngredientModel> GetIngredients()
        {
            List<IngredientModel> ingredients = new List<IngredientModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Ingredient").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (IngredientModel model in JsonConvert.DeserializeObject<List<IngredientModel>>(result))
                            {
                                ingredients.Add(model);
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
        public IEnumerable<RecipeModel> GetLastPublished()
        {
            List<RecipeModel> recipes = new List<RecipeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Recipe/GetLastPublished").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (RecipeModel model in JsonConvert.DeserializeObject<List<RecipeModel>>(result))
                            {
                                recipes.Add(model);
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
        public IEnumerable<IngredientViewModel> GetLinkedIngredients(int recipeId)
        {
            List<IngredientViewModel> viewModels = new List<IngredientViewModel>();
            DataWrapper<IngredientModel> wrapper = new DataWrapper<IngredientModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Ingredient/GetRecipeIngredients?id={recipeId}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        wrapper = JsonConvert.DeserializeObject<DataWrapper<IngredientModel>>(result);
                        if (wrapper.container.entities.Count > 0)
                        {
                            foreach (IngredientModel model in wrapper.container.entities)
                            {
                                var link = GetRecipeIngredientLink(model.Id, recipeId);
                                viewModels.Add(new IngredientViewModel()
                                {
                                    Name = model.Name,
                                    Quantity = link.Quantity,
                                    Unit = link.Unit,
                                    LinkId = link.Id
                                });
                            }
                        }
                        else return null;
                    }
                }
                return viewModels;
            }
            catch
            {
                // LOG ERROR
                return null;
            }
        }
        public IEnumerable<StepModel> GetLinkedSteps(int recipeId)
        {
            List<StepModel> items = new List<StepModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Step/GetSteps?recipeId={recipeId}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (StepModel model in JsonConvert.DeserializeObject<List<StepModel>>(result))
                            {
                                items.Add(model);
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
        public OriginModel GetOrigin(int? originId)
        {
            if (originId != null)
            {
                OriginModel item = new OriginModel();

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Origin/{originId}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null) item = JsonConvert.DeserializeObject<OriginModel>(result);
                        else return null;
                    }
                }
                return item;
            }
            else return null;
        }
        public IEnumerable<OriginModel> GetOrigins()
        {
            List<OriginModel> items = new List<OriginModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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
        public RecipeModel GetRecipe(int recipeId)
        {
            RecipeModel item = new RecipeModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                HttpResponseMessage Res = client.GetAsync($"api/Recipe/{recipeId}").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    if (result != null) item = JsonConvert.DeserializeObject<RecipeModel>(result);
                    else return null;
                }
            }
            return item;
        }
        public Recipe_IngredientModel GetRecipeIngredientLink(int ingredientId, int recipeId)
        {
            Recipe_IngredientModel item = new Recipe_IngredientModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                HttpResponseMessage Res = client.GetAsync($"api/Recipe_Ingredients/GetLink?ingredientId={ingredientId}&recipeId={recipeId}").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    if (result != null) item = JsonConvert.DeserializeObject<Recipe_IngredientModel>(result);
                    else return null;
                }
            }
            return item;
        }
        public ThemeModel GetTheme(int themeId)
        {
            ThemeModel item = new ThemeModel();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                HttpResponseMessage Res = client.GetAsync($"api/Theme/{themeId}").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    if (result != null) item = JsonConvert.DeserializeObject<ThemeModel>(result);
                    else return null;
                }
            }
            return item;
        }
        public IEnumerable<ThemeModel> GetThemes()
        {
            List<ThemeModel> items = new List<ThemeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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
        public IEnumerable<RecipeModel> GetUserRecipes()
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
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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
        public CreateRecipeModel MapCollectionToRecipeModel(HttpRequestBase request, FormCollection collection, int creatorId)
        {
            CreateRecipeModel model = new CreateRecipeModel()
            {
                CreationDate = DateTime.Now,
                CreatorId = creatorId,
                CategoryId = int.Parse(request.Form["CategoryId"]),
                OriginId = request.Form["OriginId"] != string.Empty ? int.Parse(request.Form["OriginId"]) as int? : null,
                ThemeId = int.Parse(request.Form["ThemeId"]),
                Title = request.Form["Title"],
                IsPublic = bool.Parse(request.Form["IsPublic"]),
                RecipeIngredients = new List<Recipe_IngredientModel>(),
                RecipeSteps = new List<StepModel>()
            };

            #region Catch ingredients result
            List<string> requestIngredientResult = new List<string>();

            int i = 0;
            while (collection["RecipeIngredients[" + i + "]"] != null)
            {
                requestIngredientResult.Add(collection["RecipeIngredients[" + i + "]"]);
                i++;
            }

            for (int index = 0; index < requestIngredientResult.Count; index++)
            {
                string[] ingredient = requestIngredientResult[index].Split(',');
                model.RecipeIngredients.Add(new Recipe_IngredientModel()
                {
                    IngredientId = int.Parse(ingredient[0]),
                    Quantity = int.Parse(ingredient[1]),
                    Unit = ingredient[2]
                });
            }

            #endregion Catch ingredients result

            #region Catch steps result
            List<string> requestStepResult = new List<string>();
            int j = 0;

            while (collection["RecipeSteps[" + j + "]"] != null)
            {
                requestStepResult.Add(collection["RecipeSteps[" + j + "]"]);
                j++;
            }

            for (int index = 0; index < requestStepResult.Count; index++)
            {
                string[] steps = requestStepResult[index].Split(',');
                model.RecipeSteps.Add(new StepModel()
                {
                    StepOrder = (index + 1),
                    Description = steps[0]
                });
            }

            #endregion Catch steps result

            return model;
        }
        public async Task<bool> RegisterIngredientsLinks(HttpClient client, int recipeId, IList<Recipe_IngredientModel> items)
        {
            bool isComplete = true;
            foreach (var item in items)
            {
                item.RecipeId = recipeId;

                StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(item));
                itemInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
                HttpResponseMessage itemRes = await client.PostAsync("api/Recipe_Ingredients", itemInsert);

                if (itemRes.IsSuccessStatusCode) continue;
                else isComplete = false;
            }
            return isComplete;
        }
        public async Task<bool> RegisterStepsLinks(HttpClient client, int recipeId, IList<StepModel> items)
        {
            bool isComplete = true;
            foreach (var item in items)
            {
                item.RecipeId = recipeId;

                StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(item));
                itemInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
                HttpResponseMessage itemRes = await client.PostAsync("api/Step", itemInsert);

                if (itemRes.IsSuccessStatusCode) continue;
                else isComplete = false;
            }
            return isComplete;
        }
    }
}