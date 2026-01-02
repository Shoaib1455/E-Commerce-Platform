using System;
using System.Collections.Generic;

namespace E_commerce_project.Models;

public partial class Usermanagement
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public DateOnly? CreatedAt { get; set; }

    public DateOnly? UpdatedAt { get; set; }

    public string? Name { get; set; }

    public string? Usertoken { get; set; }

    public bool Isexpired { get; set; }

    public string? Passwordresettoken { get; set; }

    public DateTime? Resettokenexpiry { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();
}
