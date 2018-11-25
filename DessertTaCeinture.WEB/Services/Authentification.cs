using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Tools;
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

        public bool LoginUser(LoginUser model)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:59049/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync($"api/User/Get/{model.Username}").Result;

                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    ModelGlobal = JsonConvert.DeserializeObject<WebApi.Models.UserModel>(Response);
                    UserModel ModelLocal = AutoMapper<WebApi.Models.UserModel, UserModel>.AutoMap(ModelGlobal);

                    string hash = BCrypt.Net.BCrypt.HashPassword(ModelLocal.Password, ModelGlobal.Salt);
                    if (BCrypt.Net.BCrypt.Verify(ModelLocal.Password, hash))
                    {
                        Session.Instance.StoreClient(ModelLocal);
                        return client != null;
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