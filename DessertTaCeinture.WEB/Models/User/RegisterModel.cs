using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.User
{
    public class RegisterModel
    {

        [Required]
        [StringLength(50)]
        [DisplayName("Adresse mail")]
        public string Email { get; set; }

        [Required]
        [DisplayName("Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "les mots de passe doivent être similaire")]
        [DisplayName("Confirmer le mot de passe")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Nom")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Prénom")]
        public string FirstName { get; set; }

        [DisplayName("Date de naissance")]
        public DateTime BirthDate { get; set; }

        [DisplayName("Sexe")]
        public bool? Gender { get; set; }
    }
}