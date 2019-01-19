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

        public void StoreUser(UserModel model)
        {
            switch (model.RoleId)
            {
                case 1:
                    HttpContext.Current.Session["loggedUser"] = model;
                    break;
                case 2:
                    HttpContext.Current.Session["loggedUser"] = model;
                    HttpContext.Current.Session["loggedModerator"] = model;
                    break;
                case 3:
                    HttpContext.Current.Session["loggedUser"] = model;
                    HttpContext.Current.Session["loggedAdmin"] = model;
                    break;
            } 
        }

        public UserModel GetConnectedUser()
        {
            return (HttpContext.Current.Session["loggedUser"] as UserModel);
        }

        public UserModel GetConnectedAdmin()
        {
            return (HttpContext.Current.Session["loggedAdmin"] as UserModel);
        }

        public void CloseSession()
        {
            HttpContext.Current.Session["loggedUser"] = null;
            HttpContext.Current.Session["loggedModerator"] = null;
            HttpContext.Current.Session["loggedAdmin"] = null;
        }
    }
}