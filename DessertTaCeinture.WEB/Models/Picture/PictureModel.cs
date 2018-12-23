using System.ComponentModel.DataAnnotations;

namespace DessertTaCeinture.WEB.Models.Picture
{
    public class PictureModel
    {
        #region Fields
        private int _Id;
        private string _Name;
        private string _Path;
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

        [Required]
        [StringLength(255)]
        public string Path
        {
            get
            {
                return _Path;
            }
            set
            {
                _Path = value;
            }
        }
        #endregion
    }
}