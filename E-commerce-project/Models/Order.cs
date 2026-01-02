using System;
using System.Collections.Generic;

namespace E_commerce_project.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public int? TotalAmount { get; set; }

    public DateTime? Createdat { get; set; }

    public DateTime? Updatedat { get; set; }

    public string? Status { get; set; }

    public int? Addressid { get; set; }

    public int? Shippingfee { get; set; }

    public string? Paymentmethod { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<Orderitem> Orderitems { get; set; } = new List<Orderitem>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
