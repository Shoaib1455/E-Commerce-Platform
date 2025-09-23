using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public bool? Isactive { get; set; }
}
