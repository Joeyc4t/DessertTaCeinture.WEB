using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Theme
{
    public class ThemeModel
    {
        #region Fields
        private int _Id;
        private string _Name;
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
        [DisplayName("Nom")]
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }
        #endregion
    }
}