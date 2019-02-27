using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Tools
{
    public class DataContainer<TEntity> where TEntity : class
    {
        public List<TEntity> entities { get; set; }
    }
}