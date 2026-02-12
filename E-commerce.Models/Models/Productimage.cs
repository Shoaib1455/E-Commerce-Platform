using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Productimage
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public string Imageurl { get; set; } = null!;

    public bool Isprimary { get; set; }

    public DateTimeOffset? Createdat { get; set; }

    public virtual Product? Product { get; set; }
}
