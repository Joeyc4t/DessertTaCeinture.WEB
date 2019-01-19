using DessertTaCeinture.WEB.Models.User;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DessertTaCeinture.WEB.Services
{
    public class User
    {
        #region Instances
        private static User _Instance;
        public static User Instance
        {
            get { return _Instance = _Instance ?? new User(); }
        }
        private User() { }
        #endregion

        public UserModel GetLoggedUser(LoginModel model)
        {
            UserModel globalModel = new UserModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/User?id={model.Email}").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        globalModel = JsonConvert.DeserializeObject<UserModel>(result);
                    }
                }
                return globalModel;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<UserModel> GetAll()
        {
            IEnumerable<UserModel> models = Enumerable.Empty<UserModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost:50140/");
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage Res = client.GetAsync($"api/User").Result;
                    if (Res.IsSuccessStatusCode)
                    {
                        var result = Res.Content.ReadAsStringAsync().Result;
                        models = JsonConvert.DeserializeObject<IEnumerable<UserModel>>(result);
                    }
                }
                return models;
            }
            catch
            {
                return null;
            }
        }
    }
}