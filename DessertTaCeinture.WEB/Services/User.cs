using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Services
{
    public class User
    {
        #region Instances
        private Session SessionService = Session.Instance;
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
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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

        public UserModel Get(string email)
        {
            UserModel globalModel = new UserModel();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

                    HttpResponseMessage Res = client.GetAsync($"api/User?id={email}").Result;
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

        public UserModel GetConnectedUser()
        {
            return SessionService.GetConnectedUser();
        }

        public IEnumerable<UserModel> GetAll()
        {
            IEnumerable<UserModel> models = Enumerable.Empty<UserModel>();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));

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

        public bool IsConnectedUser()
        {
            if (SessionService.GetConnectedUser() != null) return true;
            else return false;
        }

        public bool IsConnectedAdmin()
        {
            if (SessionService.GetConnectedAdmin() != null) return true;
            else return false;
        }
    }
}