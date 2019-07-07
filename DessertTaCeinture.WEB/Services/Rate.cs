using DessertTaCeinture.WEB.Models.Rate;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DessertTaCeinture.WEB.Services
{
    public class Rate
    {
        #region Instances
        private static Rate _Instance;
        public static Rate Instance
        {
            get { return _Instance = _Instance ?? new Rate(); }
        }
        private Rate() { }
        #endregion

        public async Task<bool> RegisterRate(HttpClient client, RateModel model)
        {
            bool isComplete;

            StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(model));
            itemInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
            HttpResponseMessage itemRes = await client.PostAsync("api/Rate", itemInsert);

            if (itemRes.IsSuccessStatusCode) isComplete = true;
            else isComplete = false;

            return isComplete;
        }
        public bool? RateExists(HttpClient client, int userId, int recipeId)
        {
            using (client)
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                HttpResponseMessage Res = client.GetAsync($"api/Rate/RateExists?userId={userId}&recipeId={recipeId}").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<bool>(result);
                }
                else return null;
            }
        }
    }
}