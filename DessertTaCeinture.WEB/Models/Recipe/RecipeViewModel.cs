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
    public class RecipeViewModel
    {
        [Required]
        [DisplayName("Catégorie")]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        [DisplayName("Rendre publique")]
        public bool IsPublic { get; set; }
        
        [DisplayName("Origine")]
        public int? OriginId { get; set; }

        [Required]
        [DisplayName("Thème")]
        public int ThemeId { get; set; }


        [Required]
        [DisplayName("Intitulé")]
        public string Title { get; set; }

        public string Picture { get; set; }

        public virtual List<OriginModel> Origins { get; set; }

        public virtual List<CategoryModel> Categories { get; set; }

        public virtual List<ThemeModel> Themes { get; set; }

        public IList<Recipe_IngredientModel> RecipeIngredients { get; set; }

        public IList<StepModel> RecipeSteps { get; set; }
    }
}