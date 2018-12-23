using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Tools
{
    public class DataContainer<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> entities { get; set; } 
    }
}