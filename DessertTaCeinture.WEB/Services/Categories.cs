using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DessertTaCeinture.WEB.Services
{
    public class Categories
    {
        #region Instances
        private Logs logsService = Logs.Instance;
        private Session SessionService = Session.Instance;

        private static Categories _Instance;
        public static Categories Instance
        {
            get { return _Instance = _Instance ?? new Categories(); }
        }
        private Categories() { }
        #endregion

        public IEnumerable<CategoryModel> GetAll()
        {
            IEnumerable<CategoryModel> models = Enumerable.Empty<CategoryModel>();

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
                        models = JsonConvert.DeserializeObject<IEnumerable<CategoryModel>>(result);
                    }
                }
                return models.OrderBy(m => m.Name);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Categories service - GetAll");
                return null;
            }
        }

        public CategoryModel Get(int id)
        {
            CategoryModel model = new CategoryModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Category?id={id}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<CategoryModel>(result);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Categories service - Get");
                return null;
            }
        }
    }
}