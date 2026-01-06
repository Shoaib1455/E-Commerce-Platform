using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Inventorytransaction
{
    public int Id { get; set; }

    public int Inventoryid { get; set; }

    public int Productid { get; set; }

    public string? Transactiontype { get; set; }

    public int? Quantity { get; set; }

    public int? Beforequantity { get; set; }

    public int? Afterquantity { get; set; }

    public string? Referencetype { get; set; }

    public int? Referenceid { get; set; }

    public string? Remarks { get; set; }

    public DateTimeOffset? Createdat { get; set; }

    public int? Createdby { get; set; }
}
