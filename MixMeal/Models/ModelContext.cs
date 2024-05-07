using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MixMeal.Models;

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

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Paymentcard> Paymentcards { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Testumonial> Testumonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userlogin> Userlogins { get; set; }

    public virtual DbSet<Userrole> Userroles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##AHMAD3;PASSWORD=Test321;DATA SOURCE=AhmadObeidat:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##AHMAD3")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008448");

            entity.ToTable("CATEGORY_");

            entity.Property(e => e.Categoryid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Categorydescription)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("CATEGORYDESCRIPTION");
            entity.Property(e => e.Categoryname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORYNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
        });

        modelBuilder.Entity<Contactu>(entity =>
        {
            entity.HasKey(e => e.Contactusid).HasName("SYS_C008473");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Contactusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Contactdate)
                .HasDefaultValueSql("SYSTIMESTAMP\n")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("CONTACTDATE");
            entity.Property(e => e.Custemail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CUSTEMAIL");
            entity.Property(e => e.Custname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CUSTNAME");
            entity.Property(e => e.Message)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Subject)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<Paymentcard>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C008444");

            entity.ToTable("PAYMENTCARD");

            entity.HasIndex(e => e.Cardnumber, "SYS_C008445").IsUnique();

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardname)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("CARDNAME");
            entity.Property(e => e.Cardnumber)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cutomerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUTOMERID");
            entity.Property(e => e.Cvc)
                .HasColumnType("NUMBER(38)")
                .HasColumnName("CVC");

            entity.HasOne(d => d.Cutomer).WithMany(p => p.Paymentcards)
                .HasForeignKey(d => d.Cutomerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CUTOMERID");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Purchaseid).HasName("SYS_C008461");

            entity.ToTable("PURCHASE");

            entity.Property(e => e.Purchaseid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PURCHASEID");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Purchasedate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("PURCHASEDATE");
            entity.Property(e => e.Recipeid)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPEID");

            entity.HasOne(d => d.Customer).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.Customerid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CUSTOMERID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.Purchases)
                .HasForeignKey(d => d.Recipeid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_RECIPEID");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Recipeid).HasName("SYS_C008456");

            entity.ToTable("RECIPE");

            entity.Property(e => e.Recipeid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPEID");
            entity.Property(e => e.Categoryid)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORYID");
            entity.Property(e => e.Chefid)
                .HasColumnType("NUMBER")
                .HasColumnName("CHEFID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(150)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Ingredients)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("INGREDIENTS");
            entity.Property(e => e.Instructions)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("INSTRUCTIONS");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.Publishdate)
                .HasDefaultValueSql("SYSTIMESTAMP  ")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("PUBLISHDATE");
            entity.Property(e => e.Recipedescription)
                .HasMaxLength(1000)
                .IsUnicode(false)
                .HasColumnName("RECIPEDESCRIPTION");
            entity.Property(e => e.Recipename)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("RECIPENAME");
            entity.Property(e => e.Recipestatusid)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPESTATUSID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CATEGORYID");

            entity.HasOne(d => d.Chef).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Chefid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CHEFID");

            entity.HasOne(d => d.Recipestatus).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Recipestatusid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_RECIPESTATUSID");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("SYS_C008419");

            entity.ToTable("STATUS");

            entity.Property(e => e.Statusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("STATUSID");
            entity.Property(e => e.Statusname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("STATUSNAME");
        });

        modelBuilder.Entity<Testumonial>(entity =>
        {
            entity.HasKey(e => e.Testumonialid).HasName("SYS_C008465");

            entity.ToTable("TESTUMONIAL");

            entity.Property(e => e.Testumonialid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTUMONIALID");
            entity.Property(e => e.Custid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTID");
            entity.Property(e => e.Posteddate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("POSTEDDATE");
            entity.Property(e => e.Testimonialstatusid)
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALSTATUSID");
            entity.Property(e => e.Ucomment)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("UCOMMENT");

            entity.HasOne(d => d.Cust).WithMany(p => p.Testumonials)
                .HasForeignKey(d => d.Custid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CUSTID");

            entity.HasOne(d => d.Testimonialstatus).WithMany(p => p.Testumonials)
                .HasForeignKey(d => d.Testimonialstatusid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TESTIMONIALSTATUSID");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008424");

            entity.ToTable("USER_");

            entity.HasIndex(e => e.Username, "USER__UK1").IsUnique();

            entity.Property(e => e.Userid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");
            entity.Property(e => e.Bithdate)
                .HasColumnType("DATE")
                .HasColumnName("BITHDATE");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Phone)
                .HasMaxLength(14)
                .IsUnicode(false)
                .HasColumnName("PHONE");
            entity.Property(e => e.Roleid)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Username)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("USERNAME");
            entity.Property(e => e.Userstatusid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERSTATUSID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("USER__FK1");

            entity.HasOne(d => d.Userstatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userstatusid)
                .HasConstraintName("USERSTATUSID__FK2");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.UserloginId).HasName("SYS_C008433");

            entity.ToTable("USERLOGIN");

            entity.HasIndex(e => e.Email, "SYS_C008434").IsUnique();

            entity.Property(e => e.UserloginId)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("USERLOGIN_ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Upassword)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("UPASSWORD");
            entity.Property(e => e.Userid)
                .HasColumnType("NUMBER")
                .HasColumnName("USERID");

            entity.HasOne(d => d.User).WithMany(p => p.Userlogins)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_USERID");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008427");

            entity.ToTable("USERROLE");

            entity.HasIndex(e => e.Rolename, "SYS_C008428").IsUnique();

            entity.Property(e => e.Roleid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ROLEID");
            entity.Property(e => e.Rolename)
                .HasMaxLength(70)
                .IsUnicode(false)
                .HasColumnName("ROLENAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
