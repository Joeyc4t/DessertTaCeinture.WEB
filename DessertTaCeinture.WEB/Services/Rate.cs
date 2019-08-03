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
        public async Task<bool> EditRate(HttpClient client, RateModel model)
        {
            client.BaseAddress = new Uri(StaticValues.BASE_URI);

            StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(model));
            toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

            HttpResponseMessage Res = await client.PutAsync($"api/Rate?id={model.Id}", toUpdate);
            if (Res.IsSuccessStatusCode) return true;
            else return false;
        }
        public async Task<bool> DeleteRate(HttpClient client, int id)
        {
            HttpResponseMessage itemRes = await client.DeleteAsync($"api/Rate?id={id}");
            return itemRes.IsSuccessStatusCode;
        }
        public bool? RateExists(int userId, int recipeId)
        {
            using (HttpClient client = new HttpClient())
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
        public RateModel GetRate(int userId, int recipeId)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                HttpResponseMessage Res = client.GetAsync($"api/Rate/GetRateByIDs?userId={userId}&recipeId={recipeId}").Result;
                if (Res.IsSuccessStatusCode)
                {
                    var result = Res.Content.ReadAsStringAsync().Result;
                    return JsonConvert.DeserializeObject<RateModel>(result);
                }
                else return null;
            }
        }
    }
}