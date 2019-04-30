using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
using DessertTaCeinture.WEB.Models.Step;
using DessertTaCeinture.WEB.Models.Theme;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Recipe
{
    public class CreateRecipeModel
    {
        public virtual List<CategoryModel> Categories { get; set; }

        [Required]
        [DisplayName("Catégorie")]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        [DisplayName("Partager")]
        public bool IsPublic { get; set; }

        [DisplayName("Provenance")]
        public int OriginId { get; set; }

        public virtual List<OriginModel> Origins { get; set; }

        public string Picture { get; set; }

        public IList<Recipe_IngredientModel> RecipeIngredients { get; set; }

        public IList<StepModel> RecipeSteps { get; set; }

        [Required]
        [DisplayName("Thématique")]
        public int ThemeId { get; set; }

        public virtual List<ThemeModel> Themes { get; set; }

        [Required]
        [DisplayName("Intitulé")]
        public string Title { get; set; }
    }
}