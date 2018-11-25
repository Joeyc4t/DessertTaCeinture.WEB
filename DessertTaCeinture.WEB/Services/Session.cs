using DessertTaCeinture.WEB.Models.User;
using System.Web;

namespace DessertTaCeinture.WEB.Services
{
    public class Session
    {
        private static Session _Instance;
        public static Session Instance
        {
            get { return _Instance = _Instance ?? new Session(); }
        }

        private Session() { }

        public void StoreClient(UserModel model)
        {
            HttpContext.Current.Session["loggedUser"] = model;
        }

        public UserModel GetConnectedClient()
        {
            return (HttpContext.Current.Session["loggedUser"] as UserModel);
        }

        public void CloseSession()
        {
            HttpContext.Current.Session["loggedUser"] = null;
        }
    }
}