using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Sales.Api.Helpers;
using Sales.Api.Models;
using Sales.Common.Models;

namespace Sales.Api.Controllers
{
    public class ProductsLuisController : ApiController
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: api/ProductsLuis
        public IQueryable<ProductsLuis> GetProductsLuis()
        {
            return this.db.ProductsLuis;
        }

        // GET: api/ProductsLuis/5
        [ResponseType(typeof(ProductsLuis))]
        public async Task<IHttpActionResult> GetProductsLuis(string id)
        {
            var productsLuis = await this.db.ProductsLuis.FindAsync(id);
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

            this.db.Entry(productsLuis).State = EntityState.Modified;

            try
            {
                await this.db.SaveChangesAsync();
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

            productsLuis.IS_AVAILABLE_BIT = true;
            productsLuis.PUBLISH_ON_DATE = DateTime.Now.ToUniversalTime();


            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (productsLuis.ImageArray != null && productsLuis.ImageArray.Length > 0)
            {
                var stream = new MemoryStream(productsLuis.ImageArray);
                var guid = Guid.NewGuid().ToString();
                var file = $"{guid}.jpg";
                var folder = "~/Content/ProductsLuis";
                var fullPath = $"{folder}/{file}";
                var response = FilesHelper.UploadPhoto(stream, folder, file);

                if (response)
                {
                    productsLuis.RUTA_IMAGEN_VAR  = fullPath;
                }
            }


            this.db.ProductsLuis.Add(productsLuis);

            try
            {
                await this.db.SaveChangesAsync();
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
            ProductsLuis productsLuis = await this.db.ProductsLuis.FindAsync(id);
            if (productsLuis == null)
            {
                return NotFound();
            }

            this.db.ProductsLuis.Remove(productsLuis);
            await this.db.SaveChangesAsync();

            return Ok(productsLuis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductsLuisExists(string id)
        {
            return this.db.ProductsLuis.Count(e => e.CVE_PRODUCTO_VAR == id) > 0;
        }
    }
}