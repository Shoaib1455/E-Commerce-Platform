using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Cart
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTimeOffset? Updatedat { get; set; }

    public bool? Isactive { get; set; }

    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual Usermanagement? User { get; set; }
}
