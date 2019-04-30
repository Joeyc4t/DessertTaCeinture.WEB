﻿using DessertTaCeinture.WEB.Models.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Controllers
{
    public class Recipe_IngredientController : Controller
    {
        public ActionResult CreateField(CreateRecipeModel model, int? index)
        {
            ViewBag.Index = index ?? 0;
            return PartialView(model);
        }
    }
}