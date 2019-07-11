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

namespace Sales.Backend.Controllers
{
    public class ProductsLuisController : Controller
    {
        private LocalDataContext db = new LocalDataContext();

        // GET: ProductsLuis
        public async Task<ActionResult> Index()
        {
            return View(await db.ProductsLuis.ToListAsync());
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
        public async Task<ActionResult> Create([Bind(Include = "CVE_PRODUCTO_VAR,NOM_PROD_VAR,PRECIO_DEC,UNIDAD_MEDIDA_VAR")] ProductsLuis productsLuis)
        {
            if (ModelState.IsValid)
            {
                db.ProductsLuis.Add(productsLuis);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productsLuis);
        }

        // GET: ProductsLuis/Edit/5
        public async Task<ActionResult> Edit(string id)
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

        // POST: ProductsLuis/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CVE_PRODUCTO_VAR,NOM_PROD_VAR,PRECIO_DEC,UNIDAD_MEDIDA_VAR")] ProductsLuis productsLuis)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsLuis).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productsLuis);
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
            ProductsLuis productsLuis = await db.ProductsLuis.FindAsync(id);
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
