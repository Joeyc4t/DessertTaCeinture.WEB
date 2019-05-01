using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
using DessertTaCeinture.WEB.Models.Step;
using DessertTaCeinture.WEB.Models.Theme;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Recipe
{
    public class CreateRecipeModel
    {
        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        [Required]
        public int ThemeId { get; set; }

        [Required]
        public string Title { get; set; }

        public int? OriginId { get; set; }

        public string Picture { get; set; }

        public OriginModel Origins { get; set; }

        public CategoryModel Categories { get; set; }

        public ThemeModel Themes { get; set; }

        public IList<Recipe_IngredientModel> RecipeIngredients { get; set; }

        public IList<StepModel> RecipeSteps { get; set; }

    }
}