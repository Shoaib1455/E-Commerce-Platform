using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Payment
{
    public long Id { get; set; }

    public string? Transactionid { get; set; }

    public string? Status { get; set; }

    public long? Amount { get; set; }

    public int? Orderid { get; set; }

    public virtual Order? Order { get; set; }
}
