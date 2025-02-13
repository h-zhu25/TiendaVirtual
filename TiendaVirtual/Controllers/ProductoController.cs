using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;                // Para Path, FileName
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;     // Ajusta a tu namespace real

namespace TiendaVirtual.Controllers
{
    public class ProductoController : Controller
    {
        private TiendaContext db = new TiendaContext();

        // GET: Producto
        public ActionResult Index()
        {
            // Listado de Productos
            return View(db.Productos.ToList());
        }

        // GET: Producto/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Producto producto = db.Productos.Find(id);
            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // GET: Producto/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "ProductoId,Nombre,Descripcion,Precio,Stock,Imagen")]
            Producto producto,
            HttpPostedFileBase imagenArchivo)
        {
            if (ModelState.IsValid)
            {
                
                if (imagenArchivo != null && imagenArchivo.ContentLength > 0)
                {
                    
                    var fileName = Path.GetFileName(imagenArchivo.FileName);
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);

                    
                    imagenArchivo.SaveAs(path);

                  
                    producto.Imagen = "/img/" + fileName;
                }
                
                db.Productos.Add(producto);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Producto producto = db.Productos.Find(id);
            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // POST: Producto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(Include = "ProductoId,Nombre,Descripcion,Precio,Stock,Imagen")]
            Producto producto,
            HttpPostedFileBase imagenArchivo)
        {
            if (ModelState.IsValid)
            {
                
                var oldData = db.Productos.AsNoTracking()
                              .FirstOrDefault(p => p.ProductoId == producto.ProductoId);

                if (oldData == null)
                    return HttpNotFound();

                
                if (imagenArchivo != null && imagenArchivo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(imagenArchivo.FileName);
                    var path = Path.Combine(Server.MapPath("~/img"), fileName);
                    imagenArchivo.SaveAs(path);

                    producto.Imagen = "/img/" + fileName;
                }
                else
                {
                    
                    producto.Imagen = oldData.Imagen;
                }

                
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(producto);
        }

        // GET: Producto/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Producto producto = db.Productos.Find(id);
            if (producto == null)
                return HttpNotFound();

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            if (producto != null)
            {
                db.Productos.Remove(producto);
                db.SaveChanges();
            }
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
