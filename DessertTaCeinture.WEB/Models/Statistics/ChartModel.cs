using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Models.Statistics
{
    public class ChartModel<T>
    {
        public ICollection<CustomData<T>> datasets { get; set; }
        public IEnumerable<string> labels { get; set; }
    }
}