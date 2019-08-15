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
        private Logs logsService = Logs.Instance;
        private Session SessionService = Session.Instance;

        private static Recipe _Instance;
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
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - DeleteIngredientsLink");
                return false;
            }            
        }
        public async Task<bool> DeleteRecipe(HttpClient client, int recipeId)
        {
            try
            {
                HttpResponseMessage itemRes = await client.DeleteAsync($"api/Recipe?id={recipeId}");
                return itemRes.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - DeleteRecipe");
                return false;
            }            
        }
        public async Task<bool> DeleteStepsLinks(HttpClient client, int recipeId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - DeleteStepsLink");
                return false;
            }            
        }
        public async Task<bool> RegisterIngredientsLinks(HttpClient client, int recipeId, IList<Recipe_IngredientModel> items)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - RegisterIngredientsLinks");
                return false;
            }            
        }
        public async Task<bool> RegisterStepsLinks(HttpClient client, int recipeId, IList<StepModel> items)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - RegisterStepsLinks");
                return false;
            }            
        }
        public async Task<bool> UpdateRecipe(HttpClient client, RecipeModel model)
        {
            try
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);

                StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(model));
                toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

                HttpResponseMessage Res = await client.PutAsync($"api/Recipe?id={model.Id}", toUpdate);
                if (Res.IsSuccessStatusCode) return true;
                else return false;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - UpdateRecipe");
                return false;
            }            
        }
        public async Task<bool> UpdateIngredientsLinks(HttpClient client, IList<Recipe_IngredientModel> items)
        {
            try
            {
                bool isComplete = true;
                foreach (var item in items)
                {
                    StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(item));
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
                    HttpResponseMessage itemRes = await client.PutAsync($"api/Recipe_Ingredients?id={item.Id}", toUpdate);

                    if (itemRes.IsSuccessStatusCode) continue;
                    else isComplete = false;
                }
                return isComplete;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - UpdateIngredientsLinks");
                return false;
            }            
        }
        public async Task<bool> UpdateStepsLinks(HttpClient client, IList<StepModel> items)
        {
            try
            {
                bool isComplete = true;
                foreach (var item in items)
                {
                    StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(item));
                    toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
                    HttpResponseMessage itemRes = await client.PutAsync($"api/Step?id={item.Id}", toUpdate);

                    if (itemRes.IsSuccessStatusCode) continue;
                    else isComplete = false;
                }
                return isComplete;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - UpdateStepsLinks");
                return false;
            }            
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetCategories");
                return null;
            }
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetIngredients");
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetLastPublished");
                return null;
            }
        }
        public IEnumerable<RecipeModel> GetPublicRecipes()
        {
            List<RecipeModel> recipes = new List<RecipeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Recipe/GetPublicRecipes").Result;
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetPublicRecipes");
                return null;
            }
        }
        public IEnumerable<RecipeModel> GetTopRecipes()
        {
            List<RecipeModel> recipes = new List<RecipeModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Rate/GetTopRecipes").Result;
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetTopRecipes");
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetLinkedIngredients");
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
                return items.OrderBy(s => s.StepOrder);
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetLinkedSteps");
                return null;
            }
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetThemes");
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetUserRecipes");
                return null;
            }
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetOrigins");
                return null;
            }
        }

        public List<Recipe_IngredientModel> GetRecipe_Ingredients(int? a, int? b, int? c, int? d, bool with)
        {
            List<Recipe_IngredientModel> items = new List<Recipe_IngredientModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Recipe_Ingredients").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (Recipe_IngredientModel origin in JsonConvert.DeserializeObject<List<Recipe_IngredientModel>>(result))
                            {
                                items.Add(origin);
                            }
                        }
                        else return null;
                    }
                }
                if(with) return items.Where(i => i.IngredientId == a || i.IngredientId == b || i.IngredientId == c || i.IngredientId == d).ToList();
                else return items.Where(i => i.IngredientId != a || i.IngredientId != b || i.IngredientId != c || i.IngredientId != d).ToList();
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetRecipe_Ingredients");
                return null;
            }
        }
        public List<Recipe_IngredientModel> GetIngredientsLinks(int recipeId)
        {
            List<Recipe_IngredientModel> items = new List<Recipe_IngredientModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Recipe_Ingredients?recipeId={recipeId}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null)
                        {
                            foreach (Recipe_IngredientModel model in JsonConvert.DeserializeObject<List<Recipe_IngredientModel>>(result))
                            {
                                items.Add(model);
                            }
                        }
                        else return null;
                    }
                }
                return items;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetIngredientsLinks");
                return null;
            }
        }
        
        public CategoryModel GetCategory(int categoryId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetCategory");
                return null;
            }            
        }
        public OriginModel GetOrigin(int? originId)
        {
            if (originId != null)
            {
                try
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
                catch(Exception ex)
                {
                    logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - UpdateStepsLinks");
                    return null;
                }                
            }
            else return null;
        }
        public RecipeModel GetRecipe(int recipeId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetRecipe");
                return null;
            }            
        }
        public Recipe_IngredientModel GetRecipeIngredientLink(int ingredientId, int recipeId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetRecipeIngredientLink");
                return null;
            }            
        }
        public ThemeModel GetTheme(int themeId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetTheme");
                return null;
            }            
        }
        public CreateRecipeModel MapCollectionToRecipeModel(HttpRequestBase request, FormCollection collection, int creatorId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - MapCollectionToRecipeModel");
                return null;
            }            
        }
        public RecipeViewModel GetRecipeFull(int recipeId)
        {
            try
            {
                RecipeModel recipeModel = GetRecipe(recipeId);

                RecipeViewModel recipeVM = new RecipeViewModel()
                {
                    Id = recipeId,
                    CategoryId = recipeModel.CategoryId,
                    CreationDate = recipeModel.CreationDate,
                    CreatorId = recipeModel.CreatorId,
                    IsPublic = recipeModel.IsPublic,
                    OriginId = recipeModel.OriginId,
                    Picture = recipeModel.Picture,
                    ThemeId = recipeModel.ThemeId,
                    Title = recipeModel.Title,
                    RecipeIngredients = GetIngredientsLinks(recipeId),
                    RecipeSteps = GetLinkedSteps(recipeId).ToList(),
                    Categories = GetCategories(),
                    Origins = GetOrigins(),
                    Themes = GetThemes()
                };

                return recipeVM;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetRecipeFull");
                return null;
            }            
        }
        public int[] GetRecipeIndexes()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Recipe/GetRecipeIndexes").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        if (result != null) return JsonConvert.DeserializeObject<int[]>(result);
                        else return null;
                    }
                    else return null;
                }
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Recipe service - GetRecipeIndexes");
                return null;
            }            
        }
    }
}