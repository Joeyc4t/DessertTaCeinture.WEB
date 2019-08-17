using DessertTaCeinture.WEB.Models.Theme;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DessertTaCeinture.WEB.Services
{
    public class Themes
    {
        #region Instances
        private Logs logsService = Logs.Instance;
        private Session SessionService = Session.Instance;

        private static Themes _Instance;
        public static Themes Instance
        {
            get { return _Instance = _Instance ?? new Themes(); }
        }
        private Themes() { }
        #endregion

        public IEnumerable<ThemeModel> GetAll()
        {
            IEnumerable<ThemeModel> models = Enumerable.Empty<ThemeModel>();

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
                        models = JsonConvert.DeserializeObject<IEnumerable<ThemeModel>>(result);
                    }
                }
                return models.OrderBy(m => m.Name);
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Themes service - GetAll");
                return null;
            }
        }

        public ThemeModel Get(int id)
        {
            ThemeModel model = new ThemeModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Theme?id={id}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<ThemeModel>(result);
                    }
                }
                return model;
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Themes service - Get");
                return null;
            }
        }
    }
}