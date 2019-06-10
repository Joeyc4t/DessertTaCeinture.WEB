using System;
using System.ComponentModel;

namespace DessertTaCeinture.WEB.Models.News
{
    public class NewsModel
    {
        #region Fields
        private int _Id;
        private string _Title;
        private string _ImageUrl;
        private string _Summary;
        private string _Description;
        private DateTime _ReleaseDate;
        #endregion

        #region Properties
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

        [DisplayName("Titre")]
        public string Title
        {
            get
            {
                return _Title;
            }
            set
            {
                _Title = value;
            }
        }

        [DisplayName("Image")]
        public string ImageUrl
        {
            get
            {
                return _ImageUrl;
            }
            set
            {
                _ImageUrl = value;
            }
        }

        [DisplayName("Résumé")]
        public string Summary
        {
            get
            {
                return _Summary;
            }
            set
            {
                _Summary = value;
            }
        }

        [DisplayName("Description")]
        public string Description
        {
            get
            {
                return _Description;
            }
            set
            {
                _Description = value;
            }
        }

        [DisplayName("Publier à partir du")]
        public DateTime ReleaseDate
        {
            get
            {
                return _ReleaseDate;
            }
            set
            {
                _ReleaseDate = value;
            }
        }
        #endregion
    }
}