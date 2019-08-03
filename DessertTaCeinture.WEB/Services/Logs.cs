using DessertTaCeinture.WEB.Models.Logs;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace DessertTaCeinture.WEB.Services
{
    public class Logs
    {
        #region Instances
        private static Logs _Instance;
        public static Logs Instance
        {
            get { return _Instance = _Instance ?? new Logs(); }
        }
        private Logs() { }
        #endregion

        public IEnumerable<LogsModel> GetAll()
        {
            IEnumerable<LogsModel> models = Enumerable.Empty<LogsModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Logs").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        models = JsonConvert.DeserializeObject<IEnumerable<LogsModel>>(result);
                    }
                }
                return models.OrderByDescending(m => m.LogDate);
            }
            catch
            {
                return null;
            }
        }
    }
}