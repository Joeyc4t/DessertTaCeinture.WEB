﻿using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Recipe_Ingredients
{
    public class Recipe_IngredientsModel
    {
        #region Fields
        private int _ConcatId;
        private int _RecipeId;
        private int _IngredientId;
        private int _Quantity;
        private string _Unit;
        #endregion

        #region Properties
        [Key]
        public int ConcatId
        {
            get
            {
                return _ConcatId;
            }
            set
            {
                _ConcatId = Convert.ToInt32((RecipeId.ToString()) + (IngredientId.ToString()));
            }
        }

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
        #endregion
    }
}