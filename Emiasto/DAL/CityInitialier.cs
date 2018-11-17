using System;
using System.Data.Entity;
using System.Collections.Generic;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Emiasto.Models;

namespace Emiasto.DAL
{
    public class CityInitialier : DropCreateDatabaseAlways<CityContext>
    {
        protected override void Seed(CityContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var roles = new List<IdentityRole>
            {
                new IdentityRole("Admin")
            };

            roleManager.Create(roles[0]);

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Firstname = "Piotr",
                    Lastname = "Świnarski",
                    Birthday = new DateTime(1994, 8, 1),
                    LockoutEnabled = true
                },
                new ApplicationUser
                {
                    UserName = "user@gmail.com",
                    Email = "user@gmail.com",
                    Firstname = "Zuzanna",
                    Lastname = "Nowak",
                    Birthday = new DateTime(1996, 12, 21),
                    LockoutEnabled = true
                }
            };

            userManager.Create(users[0], "$Admin123");
            userManager.AddToRole(users[0].Id, roles[0].Name);

            userManager.Create(users[1], "$User123");

            var categories = new List<Category>()
            {
                new Category { Name = "Gastronomia" },
                new Category { Name = "Pizzerie" },
                new Category { Name = "Handel" },
                new Category { Name = "Centra handlowe" }
            };

            categories[1].Parent = categories[0];
            categories[3].Parent = categories[2];

            categories.ForEach(category => context.Categories.Add(category));

            var facilities = new List<Facility>()
            {
                new Facility
                {
                    Name = "Galeria Siedlce",
                    Address = "Piłsudskiego 74, 08-100 Siedlce",
                    Website = "http://galeriasiedlce.pl",
                    Phone = "25 786 35 70",
                    Category = categories[3]
                },
                new Facility
                {
                    Name = "Da Grasso",
                    Address = "Henryka Sienkiewicza 12, 08-100 Siedlce",
                    Website = "http://www.dagrasso.pl/siedlce",
                    Phone = "504 908 270",
                    Category = categories[1]
                }
            };

            facilities.ForEach(facility => context.Facilities.Add(facility));

            var facilityImages = new List<FacilityImage>()
            {
                new FacilityImage { Image = "galeria-siedlce.jpg", Facility = facilities[0] },
                new FacilityImage { Image = "galeria-siedlce-2.jpg", Facility = facilities[0] },
                new FacilityImage { Image = "da-grasso.jpg", Facility = facilities[1] }
            };

            facilityImages.ForEach(facilityImage => context.FacilityImages.Add(facilityImage));

            var reviews = new List<Review>()
            {
                new Review
                {
                    Rating = 4.5,
                    Text = "Bardzo podoba mi się ta galeria. Jest w niej dużo sklepów i zawsze mogę znaleść coś dla siebie.",
                    Release = DateTime.Now,
                    Facility = facilities[0],
                    UserId = users[1].Id
                },
                new Review
                {
                    Rating = 4,
                    Text = "Fajna galeria.",
                    Release = DateTime.Now.AddDays(-1),
                    Facility = facilities[0],
                    UserId = users[1].Id
                },
                new Review
                {
                    Rating = 3.5,
                    Text = "Taka sobie.",
                    Release = DateTime.Now.AddDays(-1),
                    Facility = facilities[0],
                    UserId = users[1].Id
                },
                new Review
                {
                    Rating = 2,
                    Text = "Zrobili płatny parking...",
                    Release = DateTime.Now.AddDays(-2),
                    Facility = facilities[0],
                    UserId = users[1].Id
                }
            };

            reviews.ForEach(review => context.Reviews.Add(review));

            var reviewImages = new List<ReviewImage>()
            {
                new ReviewImage { Image = "galeria-siedlce-3.jpg", Review = reviews[0] },
                new ReviewImage { Image = "galeria-siedlce.jpg", Review = reviews[2] },
                new ReviewImage { Image = "galeria-siedlce-2.jpg", Review = reviews[2] }
            };

            reviewImages.ForEach(reviewImage => context.ReviewImages.Add(reviewImage));

            var reviewLikes = new List<ReviewLike>()
            {
                new ReviewLike { UserId = users[0].Id, Review = reviews[0] }
            };

            reviewLikes.ForEach(reviewLike => context.ReviewLikes.Add(reviewLike));

            context.SaveChanges();
        }
    }
}
