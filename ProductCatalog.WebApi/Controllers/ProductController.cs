using System;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;

using ProductCatalog.Data;
using ProductCatalog.Data.Models.Entities;

namespace ProductCatalog.WebApi.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IRepository<Product> _productRepository;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        // GET: api/Product
        public IList<Product> GetProducts()
        {
            return _productRepository.ToList();
        }

        // GET: api/ProductByQuery/query
        public IList<Product> GetProductsByQuery(string query)
        {
            return _productRepository.Get(p => p.Name.ToLower().Contains(query.ToLower()));
        }

        // GET: api/Product/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult GetProduct(int id)
        {
            var product = _productRepository.First(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // PUT: api/Product/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.Id)
            {
                return BadRequest();
            }

            product.LastUpdated = DateTime.UtcNow;

            _productRepository.Update(product);

            try
            {
                _productRepository.Commit();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Product
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Photo = "default.jpg";
            product.LastUpdated = DateTime.UtcNow;

            _productRepository.Add(product);
            _productRepository.Commit();

            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // DELETE: api/Product/5
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = _productRepository.First(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            _productRepository.Delete(product);
            _productRepository.Commit();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _productRepository.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return _productRepository.Any(p => p.Id == id);
        }
    }
}