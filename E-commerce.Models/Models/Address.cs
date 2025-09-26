using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Address
{
    public int Id { get; set; }

    public string Street { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? State { get; set; }

    public int? Postalcode { get; set; }

    public string Country { get; set; } = null!;

    public bool? Isdefault { get; set; }

    public int? Userid { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Usermanagement? User { get; set; }
}
