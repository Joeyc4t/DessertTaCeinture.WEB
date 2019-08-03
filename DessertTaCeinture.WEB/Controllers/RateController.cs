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
        public async Task<PartialViewResult> Create(int recipeId, int userId, string commentary, int rateOnFive)
        {
            RateModel model = new RateModel()
            {
                RecipeId = recipeId,
                UserId = userId,
                Commentary = commentary,
                Date = DateTime.Now,
                RateOnFive = rateOnFive
            };

            using(HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(StaticValues.BASE_URI);
                try
                {
                    bool rateRegistered = await RateService.RegisterRate(client, model);
                    if(rateRegistered) return PartialView("_EditRate", model);
                    else return PartialView("_RateFail");
                }
                catch(Exception e)
                {
                    return null;
                }
            }
        }

        public ActionResult Edit(int recipeId)
        {
            RateModel model = RateService.GetRate(SessionService.GetConnectedUser().Id, recipeId);
            return PartialView("_EditRate", model);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(int recipeId, int userId, string commentary, int rateOnFive)
        {
            bool rateUpdated; 

            RateModel oldModel = RateService.GetRate(userId, recipeId);
            RateModel updatedModel = oldModel;
            updatedModel.Date = DateTime.Now;
            updatedModel.Commentary = commentary;
            updatedModel.RateOnFive = rateOnFive;

            try
            {
                using(HttpClient client = new HttpClient())
                {
                    rateUpdated = await RateService.EditRate(client, updatedModel); 
                }

                if (rateUpdated) return PartialView("_EditRate", updatedModel);
                else return RedirectToAction("Error", "Home");
            }
            catch
            {
                return PartialView("_EditRate", oldModel);
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int recipeId, int userId)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    RateModel oldRate = RateService.GetRate(userId, recipeId);

                    client.BaseAddress = new Uri(StaticValues.BASE_URI);
                    if (await RateService.DeleteRate(client, oldRate.Id))
                    {
                        RateModel model = new RateModel()
                        {
                            RecipeId = oldRate.RecipeId,
                            UserId = oldRate.UserId
                        };
                        return PartialView("_NewRate", model);
                    }
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
