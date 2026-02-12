using System;
using System.Collections.Generic;
using E_commerce.Models.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.Models.Data;

public partial class EcommerceContext : DbContext
{
    public EcommerceContext()
    {
    }

    public EcommerceContext(DbContextOptions<EcommerceContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Cartitem> Cartitems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Inventorytransaction> Inventorytransactions { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productimage> Productimages { get; set; }

    public virtual DbSet<Usermanagement> Usermanagements { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Database=Ecommerce;Username=postgres;Password=admin");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_id");

            entity.ToTable("address");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(190L, null, null, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.City)
                .HasMaxLength(60)
                .HasColumnName("city");
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .HasColumnName("country");
            entity.Property(e => e.Isdefault).HasColumnName("isdefault");
            entity.Property(e => e.Phone).HasColumnName("phone");
            entity.Property(e => e.Postalcode).HasColumnName("postalcode");
            entity.Property(e => e.State)
                .HasMaxLength(70)
                .HasColumnName("state");
            entity.Property(e => e.Street)
                .HasMaxLength(150)
                .HasColumnName("street");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_userid");
        });

        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_cart");

            entity.ToTable("cart");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Isactive)
                .HasDefaultValue(true)
                .HasColumnName("isactive");
            entity.Property(e => e.Status)
                .HasMaxLength(15)
                .HasColumnName("status");
            entity.Property(e => e.Updatedat)
                .HasColumnType("time with time zone")
                .HasColumnName("updatedat");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.User).WithMany(p => p.Carts)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("fk_userid");
        });

        modelBuilder.Entity<Cartitem>(entity =>
        {
            entity.HasKey(e => e.Cartitemid).HasName("pk_cartitemid");

            entity.ToTable("cartitems");

            entity.HasIndex(e => new { e.Cartid, e.Productid }, "cart_product_unique").IsUnique();

            entity.Property(e => e.Cartitemid)
                .UseIdentityAlwaysColumn()
                .HasColumnName("cartitemid");
            entity.Property(e => e.Cartid).HasColumnName("cartid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Unitprice).HasColumnName("unitprice");

            entity.HasOne(d => d.Cart).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.Cartid)
                .HasConstraintName("fk_cartid");

            entity.HasOne(d => d.Product).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_productid");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(500L, null, null, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .HasColumnName("description");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(60)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_inventory");

            entity.ToTable("inventory");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Lastupdatedat).HasColumnName("lastupdatedat");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantityinstock).HasColumnName("quantityinstock");
            entity.Property(e => e.Reorderlevel).HasColumnName("reorderlevel");
            entity.Property(e => e.Reservedquantity).HasColumnName("reservedquantity");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk-product");
        });

        modelBuilder.Entity<Inventorytransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_inventorytransaction");

            entity.ToTable("inventorytransaction");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Afterquantity).HasColumnName("afterquantity");
            entity.Property(e => e.Beforequantity).HasColumnName("beforequantity");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Createdby).HasColumnName("createdby");
            entity.Property(e => e.Inventoryid).HasColumnName("inventoryid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Referenceid).HasColumnName("referenceid");
            entity.Property(e => e.Referencetype)
                .HasMaxLength(200)
                .HasColumnName("referencetype");
            entity.Property(e => e.Remarks)
                .HasMaxLength(255)
                .HasColumnName("remarks");
            entity.Property(e => e.Transactiontype)
                .HasColumnType("character varying")
                .HasColumnName("transactiontype");

            entity.HasOne(d => d.Inventory).WithMany(p => p.Inventorytransactions)
                .HasForeignKey(d => d.Inventoryid)
                .HasConstraintName("fk_inventoryid");

            entity.HasOne(d => d.Product).WithMany(p => p.Inventorytransactions)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_productid");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("order_pkey");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(60L, null, null, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Addressid).HasColumnName("addressid");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Paymentmethod)
                .HasMaxLength(6)
                .HasColumnName("paymentmethod");
            entity.Property(e => e.Shippingfee).HasColumnName("shippingfee");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasColumnName("status");
            entity.Property(e => e.TotalAmount).HasColumnName("totalAmount");
            entity.Property(e => e.Updatedat).HasColumnName("updatedat");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Addressid)
                .HasConstraintName("fk-addressid");
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orderitems_pkey");

            entity.ToTable("orderitems");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(750L, null, null, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Productid).HasColumnName("productid");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Totalprice).HasColumnName("totalprice");
            entity.Property(e => e.Unitprice).HasColumnName("unitprice");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_orderid");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderitems)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_productid");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("payment_pkey");

            entity.ToTable("payment");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(95L, null, null, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Orderid).HasColumnName("orderid");
            entity.Property(e => e.Status)
                .HasColumnType("character varying")
                .HasColumnName("status");
            entity.Property(e => e.Transactionid)
                .HasMaxLength(100)
                .HasColumnName("transactionid");

            entity.HasOne(d => d.Order).WithMany(p => p.Payments)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("fk_orderid");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("product_pkey");

            entity.ToTable("product");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasIdentityOptions(150L, null, null, null, null, null)
                .HasColumnName("id");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Createdat).HasColumnName("createdat");
            entity.Property(e => e.Description)
                .HasMaxLength(300)
                .HasColumnName("description");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(300)
                .HasColumnName("imageurl");
            entity.Property(e => e.Isactive).HasColumnName("isactive");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Price).HasColumnName("price");
            entity.Property(e => e.Sku)
                .HasMaxLength(50)
                .HasColumnName("sku");
            entity.Property(e => e.Updatedat).HasColumnName("updatedat");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("fk-categoryid");
        });

        modelBuilder.Entity<Productimage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_imagesid");

            entity.ToTable("productimages");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.Createdat)
                .HasColumnType("time with time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Imageurl)
                .HasMaxLength(255)
                .HasColumnName("imageurl");
            entity.Property(e => e.Isprimary)
                .HasDefaultValue(false)
                .HasColumnName("isprimary");
            entity.Property(e => e.Productid).HasColumnName("productid");

            entity.HasOne(d => d.Product).WithMany(p => p.Productimages)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("fk_productid");
        });

        modelBuilder.Entity<Usermanagement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("usermanagement_pkey");

            entity.ToTable("usermanagement");

            entity.Property(e => e.Id)
                .UseIdentityAlwaysColumn()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("createdAt");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Isexpired)
                .HasDefaultValue(false)
                .HasColumnName("isexpired");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Password)
                .HasMaxLength(250)
                .HasColumnName("password");
            entity.Property(e => e.Passwordresettoken)
                .HasMaxLength(100)
                .HasDefaultValueSql("NULL::character varying")
                .HasColumnName("passwordresettoken");
            entity.Property(e => e.Resettokenexpiry).HasColumnName("resettokenexpiry");
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role");
            entity.Property(e => e.UpdatedAt).HasColumnName("updatedAt");
            entity.Property(e => e.Usertoken)
                .HasMaxLength(3500)
                .HasColumnName("usertoken");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
