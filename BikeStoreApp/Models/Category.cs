﻿using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int Categoryid { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
