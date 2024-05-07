using System;
using System.Collections.Generic;

namespace MixMeal.Models;

public partial class User
{
    public decimal Userid { get; set; }

    public string Firstname { get; set; } = null!;

    public string Lastname { get; set; } = null!;

    public string? Imagepath { get; set; }

    public DateTime? Bithdate { get; set; }

    public string Phone { get; set; } = null!;

    public string Username { get; set; } = null!;

    public decimal Roleid { get; set; }

    public decimal Userstatusid { get; set; }

    public virtual ICollection<Paymentcard> Paymentcards { get; set; } = new List<Paymentcard>();

    public virtual ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

    public virtual ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();

    public virtual Userrole Role { get; set; } = null!;

    public virtual ICollection<Testumonial> Testumonials { get; set; } = new List<Testumonial>();

    public virtual ICollection<Userlogin> Userlogins { get; set; } = new List<Userlogin>();

    public virtual Status Userstatus { get; set; } = null!;
}
