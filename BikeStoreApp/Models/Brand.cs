using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public int Brandid { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Img { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
