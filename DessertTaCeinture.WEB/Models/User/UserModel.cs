using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.User
{
    public class UserModel
    {
        #region Fields
        private int _Id;
        private string _Email;
        private string _Password;
        private string _Salt;
        private string _LastName;
        private string _FirstName;
        private bool? _Gender;
        private DateTime? _InscriptionDate;
        private bool? _IsActive;
        private int? _RoleId;
        #endregion

        #region Properties
        [Key]
        public int Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
            }
        }

        [Required]
        [StringLength(50)]
        [DisplayName("Adresse mail")]
        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        [Required]
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }

        [Required]
        [StringLength(90)]
        public string Salt
        {
            get
            {
                return _Salt;
            }
            set
            {
                _Salt = value;
            }
        }

        [Required]
        [StringLength(50)]
        [DisplayName("Nom de famille")]
        public string LastName
        {
            get
            {
                return _LastName;
            }
            set
            {
                _LastName = value;
            }
        }

        [Required]
        [StringLength(50)]
        [DisplayName("Prénom")]
        public string FirstName
        {
            get
            {
                return _FirstName;
            }
            set
            {
                _FirstName = value;
            }
        }
        
        [DisplayName("Genre")]
        public bool? Gender
        {
            get
            {
                return _Gender;
            }
            set
            {
                _Gender = value;
            }
        }

        [Required]
        [DisplayName("Date d'inscription")]
        public DateTime? InscriptionDate
        {
            get
            {
                return _InscriptionDate;
            }
            set
            {
                _InscriptionDate = value;
            }
        }

        [Required]
        [DisplayName("Profil actif")]
        public bool? IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                _IsActive = value;
            }
        }

        [Required]
        public int? RoleId
        {
            get
            {
                return _RoleId;
            }
            set
            {
                _RoleId = value;
            }
        }
        #endregion
    }
}