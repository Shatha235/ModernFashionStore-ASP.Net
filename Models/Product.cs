using System;
using System.Collections.Generic;

namespace OnlineStore.Models;

public partial class Product
{
    public decimal Productid { get; set; }

    public string Name { get; set; } = null!;

    public decimal Price { get; set; }

    public decimal? Categoryid { get; set; }

    public string? Productstatus { get; set; }

    public decimal Quantity { get; set; }

    public string? Description { get; set; }

    public decimal? Producttypeid { get; set; }

    public DateTime Arrivaldate { get; set; }

    public string? Imagepath { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Favoriteitem> Favoriteitems { get; set; } = new List<Favoriteitem>();

    public virtual ICollection<Orderdetail> Orderdetails { get; set; } = new List<Orderdetail>();

    public virtual ICollection<Productattribute> Productattributes { get; set; } = new List<Productattribute>();

    public virtual ICollection<Productimage> Productimages { get; set; } = new List<Productimage>();

    public virtual Producttype? Producttype { get; set; }

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Shoppingcart> Shoppingcarts { get; set; } = new List<Shoppingcart>();

    public virtual ICollection<Userreview> Userreviews { get; set; } = new List<Userreview>();

    public string TruncatedDescription
    {
        get
        {
            const int maxLength = 100; // Adjust the maximum length as needed
            return Description.Length <= maxLength ? Description : Description.Substring(0, maxLength) + " ...";
        }
    }

}
