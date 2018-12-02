using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.User
{
    public class EditPwdModel
    {
        public int Id { get; set; }

        [Required]
        [DisplayName("Ancien mot de passe")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [DisplayName("Nouveau mot de passe")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DisplayName("Confirmez le mot de passe")]
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword), ErrorMessage = "Les mots de passe doivent être similaire")]
        public string ConfirmPassword { get; set; }
    }
}