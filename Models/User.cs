using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineStore.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string? Usercity { get; set; }

    public string? Phonenumber { get; set; }

    public string? Imagepath { get; set; }

    public string? Fname { get; set; }

    public string? Lname { get; set; }

    public string? Gender { get; set; }

    public DateTime? Birthdate { get; set; }

    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    public virtual ICollection<Favoriteitem> Favoriteitems { get; set; } = new List<Favoriteitem>();

    public virtual ICollection<Logininfo> Logininfos { get; set; } = new List<Logininfo>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Shoppingcart> Shoppingcarts { get; set; } = new List<Shoppingcart>();

    public virtual ICollection<Userreview> Userreviews { get; set; } = new List<Userreview>();

    public static int CalculateAge(DateTime birthdate)
    {
        var today = DateTime.Today;
        var age = today.Year - birthdate.Year;
        if (birthdate.Date > today.AddYears(-age)) age--;
        return age;
    }
}
