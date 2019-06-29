using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Step;
using DessertTaCeinture.WEB.Models.Theme;
using DessertTaCeinture.WEB.Models.User;

using System;
using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Models.Recipe
{
    public class RecipeDetailViewModel
    {
        public int Id { get; set; }

        public CategoryModel Category { get; set; }

        public DateTime CreationDate { get; set; }

        public UserModel Creator { get; set; }

        public OriginModel Origin { get; set; }

        public string Picture { get; set; }

        public IEnumerable<IngredientViewModel> RecipeIngredients { get; set; }

        public IEnumerable<StepModel> RecipeSteps { get; set; }

        public ThemeModel Theme { get; set; }

        public string Title { get; set; }
    }
}