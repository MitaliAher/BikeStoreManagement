using System;
using System.Collections.Generic;

namespace BikeStoreApp.Models
{
    public partial class User
    {
        public User()
        {
            Passwordresets = new HashSet<Passwordreset>();
        }

        public int Userid { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Contactno { get; set; } = null!;
        public string? City { get; set; }
        public string? Gender { get; set; }
        public bool Isadmin { get; set; }
        public bool Isstaff { get; set; }

        public virtual ICollection<Passwordreset> Passwordresets { get; set; }
    }
}
