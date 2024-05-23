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

    public virtual DbSet<Aboutpagecontent> Aboutpagecontents { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contactu> Contactus { get; set; }

    public virtual DbSet<Contactuspagecontent> Contactuspagecontents { get; set; }

    public virtual DbSet<Gender> Genders { get; set; }

    public virtual DbSet<Homepagecontent> Homepagecontents { get; set; }

    public virtual DbSet<Paymentcard> Paymentcards { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    public virtual DbSet<Testimonialpagecontent> Testimonialpagecontents { get; set; }

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

        modelBuilder.Entity<Aboutpagecontent>(entity =>
        {
            entity.HasKey(e => e.Aboutpagecontentid).HasName("SYS_C008389");

            entity.ToTable("ABOUTPAGECONTENT");

            entity.Property(e => e.Aboutpagecontentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ABOUTPAGECONTENTID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Contenttype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTENTTYPE");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Position)
                .HasColumnType("NUMBER")
                .HasColumnName("POSITION");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Categoryid).HasName("SYS_C008351");

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
            entity.HasKey(e => e.Contactusid).HasName("SYS_C008377");

            entity.ToTable("CONTACTUS");

            entity.Property(e => e.Contactusid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSID");
            entity.Property(e => e.Contactdate)
                .HasDefaultValueSql("SYSTIMESTAMP\n   ")
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
                .HasColumnType("CLOB")
                .HasColumnName("MESSAGE");
            entity.Property(e => e.Subject)
                .HasMaxLength(40)
                .IsUnicode(false)
                .HasColumnName("SUBJECT");
        });

        modelBuilder.Entity<Contactuspagecontent>(entity =>
        {
            entity.HasKey(e => e.Contactuspagecontentid).HasName("SYS_C008378");

            entity.ToTable("CONTACTUSPAGECONTENT");

            entity.Property(e => e.Contactuspagecontentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CONTACTUSPAGECONTENTID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Contenttype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTENTTYPE");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Position)
                .HasColumnType("NUMBER")
                .HasColumnName("POSITION");
        });

        modelBuilder.Entity<Gender>(entity =>
        {
            entity.HasKey(e => e.Genderid).HasName("GENDER_PK");

            entity.ToTable("GENDER");

            entity.HasIndex(e => e.Gendername, "SYS_C008334").IsUnique();

            entity.Property(e => e.Genderid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("GENDERID");
            entity.Property(e => e.Gendername)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("GENDERNAME");
        });

        modelBuilder.Entity<Homepagecontent>(entity =>
        {
            entity.HasKey(e => e.Homepagecontentid).HasName("SYS_C008356");

            entity.ToTable("HOMEPAGECONTENT");

            entity.Property(e => e.Homepagecontentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("HOMEPAGECONTENTID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Contenttype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTENTTYPE");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Position)
                .HasColumnType("NUMBER")
                .HasColumnName("POSITION");
        });

        modelBuilder.Entity<Paymentcard>(entity =>
        {
            entity.HasKey(e => e.Cardid).HasName("SYS_C008367");

            entity.ToTable("PAYMENTCARD");

            entity.Property(e => e.Cardid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("CARDID");
            entity.Property(e => e.Balance)
                .HasColumnType("FLOAT")
                .HasColumnName("BALANCE");
            entity.Property(e => e.Cardname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CARDNAME");
            entity.Property(e => e.Cardnumber)
                .HasMaxLength(16)
                .IsUnicode(false)
                .HasColumnName("CARDNUMBER");
            entity.Property(e => e.Cvv)
                .HasMaxLength(3)
                .IsUnicode(false)
                .HasColumnName("CVV");
            entity.Property(e => e.Expiredate)
                .HasColumnType("DATE")
                .HasColumnName("EXPIREDATE");
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity.HasKey(e => e.Purchaseid).HasName("SYS_C008382");

            entity.ToTable("PURCHASE");

            entity.Property(e => e.Purchaseid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("PURCHASEID");
            entity.Property(e => e.Customerid)
                .HasColumnType("NUMBER")
                .HasColumnName("CUSTOMERID");
            entity.Property(e => e.Earnings)
                .HasColumnType("FLOAT")
                .HasColumnName("EARNINGS");
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
            entity.HasKey(e => e.Recipeid).HasName("SYS_C008338");

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
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Ingredientname)
                .HasColumnType("CLOB")
                .HasColumnName("INGREDIENTNAME");
            entity.Property(e => e.Instructions)
                .HasColumnType("CLOB")
                .HasColumnName("INSTRUCTIONS");
            entity.Property(e => e.Price)
                .HasColumnType("FLOAT")
                .HasColumnName("PRICE");
            entity.Property(e => e.Publishdate)
                .HasDefaultValueSql("SYSTIMESTAMP")
                .HasColumnType("TIMESTAMP(6) WITH TIME ZONE")
                .HasColumnName("PUBLISHDATE");
            entity.Property(e => e.Recipedescription)
                .HasColumnType("CLOB")
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
                .HasConstraintName("FK_CHEFID");

            entity.HasOne(d => d.Recipestatus).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.Recipestatusid)
                .HasConstraintName("FK_RECIPESTATUSID");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Statusid).HasName("SYS_C008355");

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

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.Testumonialid).HasName("SYS_C008390");

            entity.ToTable("TESTIMONIAL");

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
                .HasMaxLength(2000)
                .IsUnicode(false)
                .HasColumnName("UCOMMENT");

            entity.HasOne(d => d.Cust).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Custid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_CUSTID");

            entity.HasOne(d => d.Testimonialstatus).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.Testimonialstatusid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK_TESTIMONIALSTATUSID");
        });

        modelBuilder.Entity<Testimonialpagecontent>(entity =>
        {
            entity.HasKey(e => e.Testimonialpagecontentid).HasName("SYS_C008357");

            entity.ToTable("TESTIMONIALPAGECONTENT");

            entity.Property(e => e.Testimonialpagecontentid)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("TESTIMONIALPAGECONTENTID");
            entity.Property(e => e.Content)
                .HasColumnType("CLOB")
                .HasColumnName("CONTENT");
            entity.Property(e => e.Contenttype)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CONTENTTYPE");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("IMAGEPATH");
            entity.Property(e => e.Position)
                .HasColumnType("NUMBER")
                .HasColumnName("POSITION");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("SYS_C008397");

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
            entity.Property(e => e.Genderid)
                .HasColumnType("NUMBER")
                .HasColumnName("GENDERID");
            entity.Property(e => e.Imagepath)
                .HasMaxLength(500)
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

            entity.HasOne(d => d.Gender).WithMany(p => p.Users)
                .HasForeignKey(d => d.Genderid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("USER__FK2");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("USER__FK1");

            entity.HasOne(d => d.Userstatus).WithMany(p => p.Users)
                .HasForeignKey(d => d.Userstatusid)
                .HasConstraintName("USERSTATUSID__FK2");
        });

        modelBuilder.Entity<Userlogin>(entity =>
        {
            entity.HasKey(e => e.UserloginId).HasName("SYS_C008348");

            entity.ToTable("USERLOGIN");

            entity.HasIndex(e => e.Email, "SYS_C008349").IsUnique();

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
                .HasConstraintName("FK_USERID");
        });

        modelBuilder.Entity<Userrole>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("SYS_C008380");

            entity.ToTable("USERROLE");

            entity.HasIndex(e => e.Rolename, "SYS_C008381").IsUnique();

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
