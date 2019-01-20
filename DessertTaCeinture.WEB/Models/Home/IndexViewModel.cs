using DessertTaCeinture.WEB.Models.News;
using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Models.Home
{
    public class IndexViewModel
    {
        public IEnumerable<NewsModel> News { get; set; }
    }
}