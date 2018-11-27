using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DessertTaCeinture.WEB.Models.User
{
    public class DetailsModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Adresse mail")]
        public string Email { get; set; }           

        [Required]
        [StringLength(50)]
        [DisplayName("Nom")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [DisplayName("Prénom")]
        public string FirstName { get; set; }

        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        [DisplayName("Date d'inscription")]
        public DateTime InscriptionDate { get; set; }
    }
}