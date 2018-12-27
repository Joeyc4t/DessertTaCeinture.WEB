using DessertTaCeinture.WEB.Models.Category;
using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Origin;
using DessertTaCeinture.WEB.Models.Theme;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Recipe
{
    public class CreateRecipeModel
    {
        [Required]
        [DisplayName("Intitulé")]
        public string Title { get; set; }
        [Required]
        public DateTime CreationDate { get; set; }
        [DisplayName("Provenance")]
        public int OriginId { get; set; }
        [Required]
        public int CreatorId { get; set; }
        [Required]
        [DisplayName("Catégorie")]
        public int CategoryId { get; set; }
        [Required]
        public int PictureId { get; set; }
        [Required]
        [DisplayName("Thématique")]
        public int ThemeId { get; set; }
        [Required]
        [DisplayName("Partager")]
        public bool IsPublic { get; set; }

        public Recipe_Ingredients.Recipe_IngredientsModel RecipeIngredient { get; set; }

        public List<OriginModel> Origins { get; set; }
        public List<ThemeModel> Themes { get; set; }
        public List<CategoryModel> Categories { get; set; }
        public List<IngredientModel> Ingredients { get; set; }
        public List<Recipe_Ingredients.Recipe_IngredientsModel> RecipeIngredients { get; set; }
    }
}