using DessertTaCeinture.WEB.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class Recipe_IngredientController : Controller
    {
        // GET: Recipe_Ingredient/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: Recipe_Ingredient/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult CreateField(CreateRecipeModel model, int? index)
        {
            ViewBag.Index = index ?? 0;
            return PartialView(model);
        }
        // GET: Recipe_Ingredient/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
        // POST: Recipe_Ingredient/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Recipe_Ingredient/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        // GET: Recipe_Ingredient/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }
        // POST: Recipe_Ingredient/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        // GET: Recipe_Ingredient
        public ActionResult Index()
        {
            return View();
        }
    }
}