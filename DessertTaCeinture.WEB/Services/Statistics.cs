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
    public class Statistics
    {
        #region Instance
        private Logs logsService = Logs.Instance;
        private Session SessionService = Session.Instance;

        private static Statistics _Instance;
        public static Statistics Instance
        {
            get { return _Instance = _Instance ?? new Statistics(); }
        }
        private Statistics() { }
        #endregion

        public Dictionary<string,int> GetChart(string id)
        {
            Dictionary<string, int> dico = new Dictionary<string, int>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Statistics/GenerateChart?id={id}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        dico = JsonConvert.DeserializeObject<Dictionary<string, int>>(result);
                    }
                }
                return dico;
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Statistics service - GetChart");
                return null;
            }
        }
    }
}