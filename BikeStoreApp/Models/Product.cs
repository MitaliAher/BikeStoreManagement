using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Product
    {
        public Product()
        {
            Orders = new HashSet<Order>();
        }

        public int Productid { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? Categoryid { get; set; }
        public int? Brandid { get; set; }
        public int? Storeid { get; set; }
        public string? Img { get; set; }

        public virtual Brand? Brand { get; set; }
        public virtual Category? Category { get; set; }
        public virtual Store? Store { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
