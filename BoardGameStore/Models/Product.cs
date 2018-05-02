using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameStore.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        //This type is nullable - I don't want price equal to 0
        public decimal? Price { get; set; }
        public string Image { get; set; }
        public string Category { get; set; }
    }
}
