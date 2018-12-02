using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View("~/Views/Shared/Errors/GenericError.cshtml");
        }

        public ActionResult Contact()
        {
            return View();
        }
    }
}