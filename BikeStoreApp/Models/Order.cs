using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? ProductId { get; set; }
        public string? CustomerName { get; set; }
        public string? Contact { get; set; }
        public string? Address { get; set; }
        public decimal? FinalAmount { get; set; }
        public int? CustomerId { get; set; }
        public string? ProductName { get; set; }

        public virtual Product? Product { get; set; }
    }
}
