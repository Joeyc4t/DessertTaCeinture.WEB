using DessertTaCeinture.WEB.Models.Logs;
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
    public class Logs
    {
        #region Instances
        private Session SessionService = Session.Instance;

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
            catch(Exception ex)
            {
                GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Logs service - Get all");
                return null;
            }
        }
        public LogsModel GetById(int id)
        {
            LogsModel model = new LogsModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Logs/{id}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        model = JsonConvert.DeserializeObject<LogsModel>(result);
                    }
                }
                return model;
            }
            catch(Exception ex)
            {
                GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Logs service - GetById");
                return null;
            }
        }
        public async Task<bool> GenerateLog(int userId, string message, string page)
        {
            LogsModel model = new LogsModel();
            model.LogGuid = Guid.NewGuid();
            model.LogDate = DateTime.Now;
            model.UserId = userId;
            model.LogMessage = message;
            model.ErrorPage = page;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);

                StringContent toInsert = new StringContent(JsonConvert.SerializeObject(model));
                toInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

                HttpResponseMessage Res = await client.PostAsync("api/Logs", toInsert);

                if (Res.IsSuccessStatusCode) return true;
                else return false;
            }
        }
    }
}