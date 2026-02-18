using System;
using System.Collections.Generic;

namespace E_commerce.Models.Models;

public partial class Paymentintent
{
    public int Id { get; set; }

    public int? Orderid { get; set; }

    public string Stripepaymentintentid { get; set; } = null!;

    public string? Clientsecret { get; set; }

    public long? Amount { get; set; }

    public string? Currency { get; set; }

    public string? Status { get; set; }

    public DateTime? Createdat { get; set; }
}
