using DessertTaCeinture.WEB.Models.Rate;
using DessertTaCeinture.WEB.Services;
using DessertTaCeinture.WEB.Tools;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class RateController : Controller
    {
        #region Instances
        Session SessionService = Services.Session.Instance;
        Rate RateService = Rate.Instance;
        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult Create(int recipeId)
        {
            RateModel model = new RateModel()
            {
                RecipeId = recipeId,
                UserId = SessionService.GetConnectedUser().Id
            };
            return PartialView("_NewRate", model);
        }

        [HttpPost]
        public async Task<PartialViewResult> Create(int recipeId, int userId, DateTime date, string commentary, int rateOnFive)
        {
            RateModel model = new RateModel()
            {
                RecipeId = recipeId,
                UserId = userId,
                Commentary = commentary,
                Date = date,
                RateOnFive = rateOnFive
            };

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                try
                {
                    bool rateRegistered = await RateService.RegisterRate(client, model);
                    if(rateRegistered) return PartialView("_RateSuccess", model);
                    else return PartialView("_RateFail");
                }
                catch(Exception e)
                {
                    return null;
                }
            }
        }

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
