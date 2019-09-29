using System;
using System.ComponentModel;
using System.Web.Mvc;

namespace DessertTaCeinture.WEB.Models.News
{
    public class NewsModel
    {
        #region Fields
        private string _Description;
        private int _Id;
        private string _ImageUrl;
        private DateTime _ReleaseDate;
        private string _Summary;
        private string _Title;

        #endregion Fields

        #region Properties

        [AllowHtml]
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

        #endregion Properties
    }
}