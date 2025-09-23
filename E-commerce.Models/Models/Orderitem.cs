using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Orderitem
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public int? Quantity { get; set; }

    public int? Unitprice { get; set; }

    public int? Totalprice { get; set; }

    public int? Orderid { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Product? Product { get; set; }
}
