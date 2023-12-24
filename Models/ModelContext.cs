using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OnlineStore.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Categorydiscount> Categorydiscounts { get; set; }

    public virtual DbSet<Categorytype> Categorytypes { get; set; }

    public virtual DbSet<Contactrequest> Contactrequests { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Delivercompany> Delivercompanies { get; set; }

    public virtual DbSet<Favoriteitem> Favoriteitems { get; set; }

    public virtual DbSet<Logininfo> Logininfos { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdelivery> Orderdeliveries { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Productattribute> Productattributes { get; set; }

    public virtual DbSet<Productcolor> Productcolors { get; set; }

    public virtual DbSet<Productimage> Productimages { get; set; }

    public virtual DbSet<Productsize> Productsizes { get; set; }

    public virtual DbSet<Producttype> Producttypes { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Shoppingcart> Shoppingcarts { get; set; }

    public virtual DbSet<Staticcard> Staticcards { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userreview> Userreviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA =(SERVER = DEDICATED)(SERVICE_NAME = XEPDB1)));User Id=FirstProject;Password=12345678;Persist Security Info=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("FIRSTPROJECT")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008429");

            entity.ToTable("CATEGORIES");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
        });

        modelBuilder.Entity<Categorydiscount>(entity =>
        {
            entity.HasKey(e => e.Discountid).HasName("SYS_C008478");

            entity.ToTable("CATEGORYDISCOUNT");

            entity.Property(e => e.Discountid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("DISCOUNTID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Discountpercentage)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("DISCOUNTPERCENTAGE");
            entity.Property(e => e.Enddate)
                .HasColumnType("DATE")
                .HasColumnName("ENDDATE");
            entity.Property(e => e.Startdate)
                .HasColumnType("DATE")
                .HasColumnName("STARTDATE");

            entity.HasOne(d => d.Category).WithMany(p => p.Categorydiscounts)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("SYS_C008479");
        });

        modelBuilder.Entity<Categorytype>(entity =>
        {
            entity.HasKey(e => e.Categorytypeid).HasName("SYS_C008531");

            entity.ToTable("CATEGORYTYPE");

            entity.Property(e => e.Categorytypeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYTYPEID");
            entity.Property(e => e.Categorytypename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORYTYPENAME");
        });

        modelBuilder.Entity<Contactrequest>(entity =>
        {
            entity.HasKey(e => e.Requestid).HasName("SYS_C008481");

            entity.ToTable("CONTACTREQUESTS");

            entity.Property(e => e.Requestid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REQUESTID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Message)
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Submissiondate)
                .HasPrecision(6)
                .HasColumnName("SUBMISSIONDATE");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Couponid).HasName("SYS_C008438");

            entity.ToTable("COUPONS");

            entity.Property(e => e.Couponid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("COUPONID");
            entity.Property(e => e.Couponcode)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COUPONCODE");
            entity.Property(e => e.Discountpercentage)
                .HasColumnType("NUMBER(5,2)")
                .HasColumnName("DISCOUNTPERCENTAGE");
            entity.Property(e => e.Enddate)
                .HasColumnType("DATE")
                .HasColumnName("ENDDATE");
            entity.Property(e => e.Startdate)
                .HasColumnType("DATE")
                .HasColumnName("STARTDATE");
        });

        modelBuilder.Entity<Delivercompany>(entity =>
        {
            entity.HasKey(e => e.Delivercompanyid).HasName("SYS_C008442");

            entity.ToTable("DELIVERCOMPANIES");

            entity.Property(e => e.Delivercompanyid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("DELIVERCOMPANYID");
            entity.Property(e => e.Delivercompanyaddress)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DELIVERCOMPANYADDRESS");
            entity.Property(e => e.Delivercompanyname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("DELIVERCOMPANYNAME");
        });

        modelBuilder.Entity<Favoriteitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008471");

            entity.ToTable("FAVORITEITEMS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Favoriteitems)
                .HasForeignKey(d => d.Productid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008473");

            entity.HasOne(d => d.User).WithMany(p => p.Favoriteitems)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008472");
        });

        modelBuilder.Entity<Logininfo>(entity =>
        {
            entity.HasKey(e => e.Loginid).HasName("SYS_C008424");

            entity.ToTable("LOGININFO");

            entity.Property(e => e.Loginid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("LOGINID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Userroleid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERROLEID");

            entity.HasOne(d => d.User).WithMany(p => p.Logininfos)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008426");

            entity.HasOne(d => d.Userrole).WithMany(p => p.Logininfos)
                .HasForeignKey(d => d.Userroleid)
                .HasConstraintName("SYS_C008425");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Orderid).HasName("SYS_C008490");

            entity.ToTable("ORDERS");

            entity.Property(e => e.Orderid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Couponid)
                .HasColumnType("NUMBER")
                .HasColumnName("COUPONID");
            entity.Property(e => e.Orderdate)
                .HasColumnType("DATE")
                .HasColumnName("ORDERDATE");
            entity.Property(e => e.Orderstatus)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ORDERSTATUS");
            entity.Property(e => e.Totalprice)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("TOTALPRICE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Coupon).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Couponid)
                .HasConstraintName("SYS_C008492");

            entity.HasOne(d => d.User).WithMany(p => p.Orders)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008491");
        });

        modelBuilder.Entity<Orderdelivery>(entity =>
        {
            entity.HasKey(e => e.Orderdeliveriesid).HasName("SYS_C008497");

            entity.ToTable("ORDERDELIVERIES");

            entity.Property(e => e.Orderdeliveriesid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERDELIVERIESID");
            entity.Property(e => e.Delivercompanyid)
                .HasColumnType("NUMBER")
                .HasColumnName("DELIVERCOMPANYID");
            entity.Property(e => e.Orderdeliverystatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("ORDERDELIVERYSTATUS");
            entity.Property(e => e.Orderid)
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");

            entity.HasOne(d => d.Delivercompany).WithMany(p => p.Orderdeliveries)
                .HasForeignKey(d => d.Delivercompanyid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008499");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdeliveries)
                .HasForeignKey(d => d.Orderid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008498");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Orderdetailid).HasName("SYS_C008484");

            entity.ToTable("ORDERDETAILS");

            entity.Property(e => e.Orderdetailid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERDETAILID");
            entity.Property(e => e.Orderid)
                .HasColumnType("NUMBER")
                .HasColumnName("ORDERID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Orderid)
                .HasConstraintName("FK_ORDERDETAILS_ORDERS");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008486");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Productid).HasName("SYS_C008433");

            entity.ToTable("PRODUCTS");

            entity.Property(e => e.Productid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Arrivaldate)
                .HasColumnType("DATE")
                .HasColumnName("ARRIVALDATE");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Description)
                .HasColumnType("CLOB")
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("PRICE");
            entity.Property(e => e.Productstatus)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("PRODUCTSTATUS");
            entity.Property(e => e.Producttypeid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTTYPEID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.Categoryid)
                .HasConstraintName("SYS_C008434");

            entity.HasOne(d => d.Producttype).WithMany(p => p.Products)
                .HasForeignKey(d => d.Producttypeid)
                .HasConstraintName("FK_PRODUCTTYPE_TYPEID");
        });

        modelBuilder.Entity<Productattribute>(entity =>
        {
            entity.HasKey(e => e.Productattributeid).HasName("SYS_C008520");

            entity.ToTable("PRODUCTATTRIBUTES");

            entity.Property(e => e.Productattributeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTATTRIBUTEID");
            entity.Property(e => e.Colorid)
                .HasColumnType("NUMBER")
                .HasColumnName("COLORID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.Sizeid)
                .HasColumnType("NUMBER")
                .HasColumnName("SIZEID");

            entity.HasOne(d => d.Color).WithMany(p => p.Productattributes)
                .HasForeignKey(d => d.Colorid)
                .HasConstraintName("SYS_C008522");

            entity.HasOne(d => d.Product).WithMany(p => p.Productattributes)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008521");

            entity.HasOne(d => d.Size).WithMany(p => p.Productattributes)
                .HasForeignKey(d => d.Sizeid)
                .HasConstraintName("SYS_C008523");
        });

        modelBuilder.Entity<Productcolor>(entity =>
        {
            entity.HasKey(e => e.Colorid).HasName("SYS_C008514");

            entity.ToTable("PRODUCTCOLOR");

            entity.Property(e => e.Colorid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("COLORID");
            entity.Property(e => e.Colorname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("COLORNAME");
        });

        modelBuilder.Entity<Productimage>(entity =>
        {
            entity.HasKey(e => e.Imageid).HasName("SYS_C008526");

            entity.ToTable("PRODUCTIMAGE");

            entity.Property(e => e.Imageid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("IMAGEID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");

            entity.HasOne(d => d.Product).WithMany(p => p.Productimages)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008527");
        });

        modelBuilder.Entity<Productsize>(entity =>
        {
            entity.HasKey(e => e.Sizeid).HasName("SYS_C008517");

            entity.ToTable("PRODUCTSIZE");

            entity.Property(e => e.Sizeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SIZEID");
            entity.Property(e => e.Sizename)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("SIZENAME");
        });

        modelBuilder.Entity<Producttype>(entity =>
        {
            entity.HasKey(e => e.Typeid).HasName("SYS_C008510");

            entity.ToTable("PRODUCTTYPE");

            entity.Property(e => e.Typeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TYPEID");
            entity.Property(e => e.Categorytypeid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYTYPEID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Typename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("TYPENAME");

            entity.HasOne(d => d.Categorytype).WithMany(p => p.Producttypes)
                .HasForeignKey(d => d.Categorytypeid)
                .HasConstraintName("FK_PRODUCTTYPE_CATEGORYTYPEID");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.Ratingid).HasName("SYS_C008465");

            entity.ToTable("RATINGS");

            entity.Property(e => e.Ratingid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RATINGID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Rating1)
                .HasColumnType("NUMBER(3,1)")
                .HasColumnName("RATING");
            entity.Property(e => e.Ratingdate)
                .HasColumnType("DATE")
                .HasColumnName("RATINGDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008467");

            entity.HasOne(d => d.User).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008466");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008417");

            entity.ToTable("ROLE");

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        modelBuilder.Entity<Shoppingcart>(entity =>
        {
            entity.HasKey(e => e.Shoppingcartid).HasName("SYS_C008457");

            entity.ToTable("SHOPPINGCART");

            entity.Property(e => e.Shoppingcartid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("SHOPPINGCARTID");
            entity.Property(e => e.Colorid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("COLORID");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Quantity)
                .HasColumnType("NUMBER")
                .HasColumnName("QUANTITY");
            entity.Property(e => e.Sizeid)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("SIZEID");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Color).WithMany(p => p.Shoppingcarts)
                .HasForeignKey(d => d.Colorid)
                .HasConstraintName("FK_SHOPPINGCART_COLOR");

            entity.HasOne(d => d.Product).WithMany(p => p.Shoppingcarts)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008459");

            entity.HasOne(d => d.Size).WithMany(p => p.Shoppingcarts)
                .HasForeignKey(d => d.Sizeid)
                .HasConstraintName("FK_SHOPPINGCART_SIZE");

            entity.HasOne(d => d.User).WithMany(p => p.Shoppingcarts)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008458");
        });

        modelBuilder.Entity<Staticcard>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C008475");

            entity.ToTable("STATICCARDS");

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Balance)
                .HasColumnType("NUMBER(10,2)")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.Expirydate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIRYDATE");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testimonialid).HasName("SYS_C008539");

            entity.ToTable("TESTIMONIALS");

            entity.Property(e => e.Testimonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALID");
            entity.Property(e => e.Approvalstatus)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasDefaultValueSql("'pending'\n")
                .HasColumnName("APPROVALSTATUS");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Feedback)
                .HasColumnType("CLOB")
                .HasColumnName("FEEDBACK");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.Submissiondate)
                .HasPrecision(6)
                .HasColumnName("SUBMISSIONDATE");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008419");

            entity.ToTable("USERS");

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Birthdate)
                .HasColumnType("DATE")
                .HasColumnName("BIRTHDATE");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GENDER");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LNAME");
            entity.Property(e => e.Phonenumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PHONENUMBER");
            entity.Property(e => e.Usercity)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("USERCITY");
        });

        modelBuilder.Entity<Userreview>(entity =>
        {
            entity.HasKey(e => e.Reviewid).HasName("SYS_C008461");

            entity.ToTable("USERREVIEWS");

            entity.Property(e => e.Reviewid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("REVIEWID");
            entity.Property(e => e.Comments)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("COMMENTS");
            entity.Property(e => e.Productid)
                .HasColumnType("NUMBER")
                .HasColumnName("PRODUCTID");
            entity.Property(e => e.Reviewdate)
                .HasColumnType("DATE")
                .HasColumnName("REVIEWDATE");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.Product).WithMany(p => p.Userreviews)
                .HasForeignKey(d => d.Productid)
                .HasConstraintName("SYS_C008463");

            entity.HasOne(d => d.User).WithMany(p => p.Userreviews)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("SYS_C008462");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
