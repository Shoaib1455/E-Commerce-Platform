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

    public string Name { get; set; } = null!;
}
