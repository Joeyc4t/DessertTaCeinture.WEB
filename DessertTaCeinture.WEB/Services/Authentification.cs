using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DessertTaCeinture.WEB.Services
{
    public class Authentification
    {
        #region Instance
        private static Authentification _Instance;
        public static Authentification Instance
        {
            get { return _Instance = _Instance ?? new Authentification(); }
        }
        private Authentification() { }
        #endregion

        public bool LoginUser(LoginModel model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(StaticValues.API_MEDIA_TYPE));
                HttpResponseMessage Res = client.GetAsync($"api/User?id={model.Email}").Result;

                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    UserModel globalModel = JsonConvert.DeserializeObject<UserModel>(Response);
                    if (globalModel.IsActive == true)
                    {
                        string hash = BCrypt.Net.BCrypt.HashPassword(model.Password, globalModel.Salt);
                        if (globalModel.Password.Equals(hash))
                        {
                            Session.Instance.StoreUser(globalModel);
                            return client != null;
                        }
                        else return false;
                    }
                    else return false;
                }
                else return false;
            }
        }

        public void Logout()
        {
            Session.Instance.CloseSession();
        }
    }
}