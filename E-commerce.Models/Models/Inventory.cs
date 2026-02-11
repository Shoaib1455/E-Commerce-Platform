using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public int? Productid { get; set; }

    public int Quantityinstock { get; set; }

    public int Reorderlevel { get; set; }

    public bool? Isactive { get; set; }

    public DateTime? Lastupdatedat { get; set; }

    public int? Reservedquantity { get; set; }

    public virtual ICollection<Inventorytransaction> Inventorytransactions { get; set; } = new List<Inventorytransaction>();

    public virtual Product? Product { get; set; }
}
