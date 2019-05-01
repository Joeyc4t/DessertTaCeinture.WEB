using DessertTaCeinture.WEB.Models.Ingredient;
using DessertTaCeinture.WEB.Models.Recipe;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Recipe_Ingredients
{
    public class Recipe_IngredientModel
    {
        #region Fields
        private int _Id;
        private int _IngredientId;
        private int _Quantity;
        private int _RecipeId;
        private string _Unit;

        #endregion Fields

        #region Properties

        [Key]
        public int ConcatId
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = Convert.ToInt32((RecipeId.ToString()) + (IngredientId.ToString()));
            }
        }

        public virtual IngredientModel Ingredient { get; set; }

        [Required]
        [DisplayName("Ingrédient")]
        public int IngredientId
        {
            get
            {
                return _IngredientId;
            }
            set
            {
                _IngredientId = value;
            }
        }

        [Required]
        [DisplayName("Quantité")]
        [Range(0, 9999)]
        public int Quantity
        {
            get
            {
                return _Quantity;
            }
            set
            {
                _Quantity = value;
            }
        }

        public virtual RecipeModel Recipe { get; set; }

        [Required]
        public int RecipeId
        {
            get
            {
                return _RecipeId;
            }
            set
            {
                _RecipeId = value;
            }
        }

        [Required]
        [StringLength(20)]
        public string Unit
        {
            get
            {
                return _Unit;
            }
            set
            {
                _Unit = value;
            }
        }

        #endregion Properties
    }
}