using DessertTaCeinture.WEB.Models.Ingredient;
using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Models.Recipe_Ingredients
{
    public class Recipe_IngredientViewModel
    {
        public List<IngredientModel> Ingredients { get; set; }
        public Recipe_IngredientsModel RecipeIngredient { get; set; }
    }
}