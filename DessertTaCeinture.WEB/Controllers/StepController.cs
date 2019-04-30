using DessertTaCeinture.WEB.Models.Recipe;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class StepController : Controller
    {
        public ActionResult CreateField(CreateRecipeModel model, int? index)
        {
            ViewBag.StepIndex = index ?? 0;
            return PartialView(model);
        }
    }
}