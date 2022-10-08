using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Xml.Linq;

namespace ASP_NetCoreProject.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> countries { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
    }
    public class City
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "City")]
        public string Name { get; set; }

        [ForeignKey("Country"), Display(Name = "Country")]
        public long CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual IList<Customer> Customers { get; set; }
        public virtual IList<Supplier> Suppliers { get; set; }
    }
    public class Country
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "Country")]
        public string Name { get; set; }

        public virtual IList<City> Cities { get; set; }
    }
    public class Customer
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        [ForeignKey("City"), Display(Name = "City")]
        public long CityId { get; set; }
        public virtual City City { get; set; }
    }

    public class Supplier
    {
        [Key]
        public long ID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Contact Name")]
        public string ContactName { get; set; }

        [Display(Name = "Contact Title")]
        public string ContactTitle { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Fax { get; set; }


        [ForeignKey("City"), Display(Name = "City")]
        public long CityId { get; set; }
        public virtual City City { get; set; }

    }
}

