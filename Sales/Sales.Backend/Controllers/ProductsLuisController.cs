using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Sales.Backend.Models;
using Sales.Common.Models;
using Sales.Backend.Helpers;

namespace Sales.Backend.Controllers
{
    public class ProductsLuisController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: ProductsLuis
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductsLuis.OrderBy(p => p.NOM_PROD_VAR).ToListAsync());
        }

        // GET: ProductsLuis/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsLuis productsLuis = await db.ProductsLuis.FindAsync(id);
            if (productsLuis == null)
            {
                return HttpNotFound();
            }
            return View(productsLuis);
        }

        // GET: ProductsLuis/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsLuis/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = string.Empty;
                var folder = "~/Content/ProductsLuis";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = this.ToProduct(view, pic);
                this.db.ProductsLuis.Add(product);
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        private ProductsLuis ToProduct(ProductView view, string pic)
        {
            return new ProductsLuis
            {
                NOM_PROD_VAR  = view.NOM_PROD_VAR ,
                RUTA_IMAGEN_VAR  = pic,
                IS_AVAILABLE_BIT  = view.IS_AVAILABLE_BIT ,
                PRECIO_DEC  = view.PRECIO_DEC ,
                CVE_PRODUCTO_VAR  = view.CVE_PRODUCTO_VAR ,
                PUBLISH_ON_DATE  = view.PUBLISH_ON_DATE ,
                REMARK_VAR  = view.REMARK_VAR,
                UNIDAD_MEDIDA_VAR = view.UNIDAD_MEDIDA_VAR ,
            };
        }


        // GET: ProductsLuis/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            }

            var product = await this.db.ProductsLuis.FindAsync(id);

            if (product == null)
            {
                return HttpNotFound();
            }

            var view = this.ToView(product);
            return View(view);
        }

        private ProductView ToView(ProductsLuis  product)
        {
            return new ProductView
            {
                NOM_PROD_VAR  = product.NOM_PROD_VAR ,
                RUTA_IMAGEN_VAR  = product.RUTA_IMAGEN_VAR ,
                IS_AVAILABLE_BIT  = product.IS_AVAILABLE_BIT ,
                PRECIO_DEC  = product.PRECIO_DEC ,
                CVE_PRODUCTO_VAR  = product.CVE_PRODUCTO_VAR ,
                PUBLISH_ON_DATE  = product.PUBLISH_ON_DATE ,
                REMARK_VAR  = product.REMARK_VAR ,
                UNIDAD_MEDIDA_VAR = product .UNIDAD_MEDIDA_VAR,
            };
        }

        // POST: ProductsLuis/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductView view)
        {
            if (ModelState.IsValid)
            {
                var pic = view.RUTA_IMAGEN_VAR ;
                var folder = "~/Content/ProductsLuis";

                if (view.ImageFile != null)
                {
                    pic = FilesHelper.UploadPhoto(view.ImageFile, folder);
                    pic = $"{folder}/{pic}";
                }

                var product = this.ToProduct(view, pic);
                this.db.Entry(product).State = EntityState.Modified;
                await this.db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(view);
        }

        // GET: ProductsLuis/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductsLuis productsLuis = await db.ProductsLuis.FindAsync(id);
            if (productsLuis == null)
            {
                return HttpNotFound();
            }
            return View(productsLuis);
        }

        // POST: ProductsLuis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            var productsLuis = await db.ProductsLuis.FindAsync(id);
            db.ProductsLuis.Remove(productsLuis);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
