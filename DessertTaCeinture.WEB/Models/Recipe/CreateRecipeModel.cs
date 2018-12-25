using DessertTaCeinture.WEB.Models.Recipe_Ingredients;
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

        List<Recipe_IngredientsModel> Ingredients { get; set; }
    }
}