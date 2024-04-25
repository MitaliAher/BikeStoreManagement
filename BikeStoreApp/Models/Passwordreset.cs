using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class Passwordreset
    {
        public int Resetid { get; set; }
        public int Userid { get; set; }
        public string Resettoken { get; set; } = null!;
        public DateTime Expirationtime { get; set; }

        public virtual User User { get; set; } = null!;
    }
}
