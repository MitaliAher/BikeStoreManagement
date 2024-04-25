using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Store
    {
        public Store()
        {
            Products = new HashSet<Product>();
        }

        public int Storeid { get; set; }
        public string? Name { get; set; }
        public string? Location { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
