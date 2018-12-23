using DessertTaCeinture.WEB.Models.Enumerations;
using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class Recipe_IngredientsController : Controller
    {
        public ActionResult Create()
        {


            return View();
        }

        public PartialViewResult AddIngredient()
        {
            return PartialView("_AddIngredient", new Recipe_IngredientsModel());
        }

        [HttpPost]
        public ActionResult Create(Recipe_IngredientsModel model)
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

        #region Private Methods

        #endregion
    }
}
