using System.Collections.Generic;

namespace DessertTaCeinture.WEB.Models.Statistics
{
    public class CustomData<T>
    {
        public IEnumerable<string> backgroundColor { get; set; }
        public IEnumerable<string> borderColor { get; set; }
        public ICollection<T> data { get; set; }
        public bool? fill { get; set; }
        public int? pointHitRadius { get; set; }
        public int? pointHoverRadius { get; set; }
        public int? pointRadius { get; set; }
        public string type { get; set; }
    }
}