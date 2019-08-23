using System;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Recipe
{
    public class RecipeModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        public int CreatorId { get; set; }

        [Required]
        public bool IsPublic { get; set; }

        public int? OriginId { get; set; }

        public string Picture { get; set; }

        [Required]
        public int ThemeId { get; set; }

        [Required]
        [StringLength(75)]
        public string Title { get; set; }

        public double? Average { get; set; }
    }
}