using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Theme;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DessertTaCeinture.WEB.Services
{
    public class Recipe
    {
        #region Instances
        private static Recipe _Instance;
        public static Recipe Instance
        {
            get { return _Instance = _Instance ?? new Recipe(); }
        }
        private Recipe() { }
        #endregion       

        public List<CategoryModel> GetCategories()
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

        public List<ThemeModel> GetThemes()
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

        public List<OriginModel> GetOrigins()
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

        public List<IngredientModel> GetIngredients()
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
    }
}