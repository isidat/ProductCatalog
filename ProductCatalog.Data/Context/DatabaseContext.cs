using System;
using System.Data.Entity;

using ProductCatalog.Data.Models.Entities;

namespace ProductCatalog.Data.Context
{
    public interface IDatabaseContext : IDisposable
    {
        IDbSet<Product> Products { get; set; }
    }

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        public DatabaseContext() : base("ProductCatalogConnection") { }

        public IDbSet<Product> Products { get; set; }
    }
}