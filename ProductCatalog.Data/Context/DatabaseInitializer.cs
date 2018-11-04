using System;
using System.Data.Entity;

using ProductCatalog.Data.Models.Entities;

namespace ProductCatalog.Data.Context
{
    public class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            base.Seed(context);

            context.Products.Add(new Product
            {
                Name = "Havana Green Suit",
                Photo = "suit.jpg",
                Price = 569,
                LastUpdated = DateTime.UtcNow
            });

            context.Products.Add(new Product
            {
                Name = "Lazio Navy Tuxedo",
                Photo = "tuxedo.jpg",
                Price = 739,
                LastUpdated = DateTime.UtcNow
            });

            context.Products.Add(new Product
            {
                Name = "Jort Brown Check Jacket",
                Photo = "jacket.jpg",
                Price = 499,
                LastUpdated = DateTime.UtcNow
            });

            context.Products.Add(new Product
            {
                Name = "Green Trousers",
                Photo = "trousers.jpg",
                Price = 269,
                LastUpdated = DateTime.UtcNow
            });

            context.SaveChanges();
        }
    }
}