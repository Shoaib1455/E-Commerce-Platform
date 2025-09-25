using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace E_commerce.Models.Models;

public partial class Cartitem
{
    public int Cartitemid { get; set; }

    public int? Cartid { get; set; }

    public int? Productid { get; set; }

    public int? Quantity { get; set; }

    public int? Unitprice { get; set; }
    [JsonIgnore]
    public virtual Cart? Cart { get; set; }

    public virtual Product? Product { get; set; }
}
