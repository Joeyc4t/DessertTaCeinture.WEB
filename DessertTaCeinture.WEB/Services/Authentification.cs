using DessertTaCeinture.WEB.Models.User;
using DessertTaCeinture.WEB.Tools;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.Results;

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
                client.BaseAddress = new Uri("http://localhost:50140/");
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage Res = client.GetAsync($"api/User/Get").Result;

                if (Res.IsSuccessStatusCode)
                {
                    var Response = Res.Content.ReadAsStringAsync().Result;
                    IQueryable<UserModel> GlobalModels = JsonConvert.DeserializeObject(Response) as IQueryable<UserModel>;

                    UserModel GlobalModel = GlobalModels.FirstOrDefault(u => u.Email == model.Email);

                    string hash = BCrypt.Net.BCrypt.HashPassword(model.Password, GlobalModel.Salt);
                    if (BCrypt.Net.BCrypt.Verify(GlobalModel.Password, hash))
                    {
                        Session.Instance.StoreUser(GlobalModel);
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