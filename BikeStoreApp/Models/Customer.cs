using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Customer
    {
        public int Customerid { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Contactno { get; set; }
    }
}
