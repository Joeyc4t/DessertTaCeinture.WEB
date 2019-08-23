using DessertTaCeinture.WEB.Models.Rate;
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
    public class Rate
    {
        #region Instances
        private Logs logsService = Logs.Instance;
        private Session SessionService = Session.Instance;

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

            try
            {
                StringContent itemInsert = new StringContent(JsonConvert.SerializeObject(model));
                itemInsert.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);
                HttpResponseMessage itemRes = await client.PostAsync("api/Rate", itemInsert);

                if (itemRes.IsSuccessStatusCode) isComplete = true;
                else isComplete = false;

                return isComplete;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Rate service - RegisterRate");
                return false;
            }            
        }
        public async Task<bool> EditRate(HttpClient client, RateModel model)
        {
            try
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);

                StringContent toUpdate = new StringContent(JsonConvert.SerializeObject(model));
                toUpdate.Headers.ContentType = new MediaTypeHeaderValue(StaticValues.API_MEDIA_TYPE);

                HttpResponseMessage Res = await client.PutAsync($"api/Rate?id={model.Id}", toUpdate);
                if (Res.IsSuccessStatusCode) return true;
                else return false;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Rate service -EditRate");
                return false;
            }            
        }
        public async Task<bool> DeleteRate(HttpClient client, int id)
        {
            try
            {
                HttpResponseMessage itemRes = await client.DeleteAsync($"api/Rate?id={id}");
                return itemRes.IsSuccessStatusCode;
            }
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Rate service - DeleteRate");
                return false;
            }            
        }
        public bool? RateExists(int userId, int recipeId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Rate service - RateExists");
                return null;
            }
            
        }
        public RateModel GetRate(int userId, int recipeId)
        {
            try
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
            catch(Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Rate service - GetRate");
                return null;
            }            
        }
        public List<RateModel> GetRates()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/Rate").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<List<RateModel>>(result);
                    }
                    else return null;
                }
            }
            catch (Exception ex)
            {
                logsService.GenerateLog(SessionService.GetConnectedUser().Id, ex.Message, "Rate service - GetRates");
                return null;
            }
        }

        public double? CalculateAverage(int recipeId)
        {
            IEnumerable<int> recipeRates = GetRates().Where(r => r.RecipeId.Equals(recipeId))
                                                     .Select(r => r.RateOnFive);

            if (recipeRates.Count() > 0) return recipeRates.Average();
            else return null;            
        }
    }
}