using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sales.Common.Models;
using Sales.Domain.Models;

namespace Sales.Api.Controllers
{
    public class ProductsLuisController : ApiController
    {
        private DataContext db = new DataContext();

        // GET: api/ProductsLuis
        public IQueryable<ProductsLuis> GetProductsLuis()
        {
            return db.ProductsLuis;
        }

        // GET: api/ProductsLuis/5
        [ResponseType(typeof(ProductsLuis))]
        public async Task<IHttpActionResult> GetProductsLuis(string id)
        {
            ProductsLuis productsLuis = await db.ProductsLuis.FindAsync(id);
            if (productsLuis == null)
            {
                return NotFound();
            }

            return Ok(productsLuis);
        }

        // PUT: api/ProductsLuis/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProductsLuis(string id, ProductsLuis productsLuis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != productsLuis.CVE_PRODUCTO_VAR)
            {
                return BadRequest();
            }

            db.Entry(productsLuis).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductsLuisExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/ProductsLuis
        [ResponseType(typeof(ProductsLuis))]
        public async Task<IHttpActionResult> PostProductsLuis(ProductsLuis productsLuis)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ProductsLuis.Add(productsLuis);

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ProductsLuisExists(productsLuis.CVE_PRODUCTO_VAR))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = productsLuis.CVE_PRODUCTO_VAR }, productsLuis);
        }

        // DELETE: api/ProductsLuis/5
        [ResponseType(typeof(ProductsLuis))]
        public async Task<IHttpActionResult> DeleteProductsLuis(string id)
        {
            ProductsLuis productsLuis = await db.ProductsLuis.FindAsync(id);
            if (productsLuis == null)
            {
                return NotFound();
            }

            db.ProductsLuis.Remove(productsLuis);
            await db.SaveChangesAsync();

            return Ok(productsLuis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsLuisExists(string id)
        {
            return db.ProductsLuis.Count(e => e.CVE_PRODUCTO_VAR == id) > 0;
        }
    }
}