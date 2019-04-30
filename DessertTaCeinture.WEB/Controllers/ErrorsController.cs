using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class ErrorsController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Une erreur est survenue";
            return View("~/Views/Shared/Errors/GenericError.cshtml");
        }

        public ActionResult NotFound()
        {
            ViewBag.Title = "Erreur 404";
            return View("~/Views/Shared/Errors/NotFound.cshtml");
        }
    }
}