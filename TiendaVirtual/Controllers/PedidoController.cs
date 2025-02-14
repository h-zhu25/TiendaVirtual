using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private TiendaContext db = new TiendaContext();

        // GET: Pedido
        public ActionResult Index()
        {
            List<Pedido> pedidos;

            
            if (User.IsInRole("Admin"))
            {
                pedidos = db.Pedidos.Include(p => p.Detalles).ToList();
            }
            else
            {
                
                var currentUser = User.Identity.Name;
                pedidos = db.Pedidos
                            .Include(p => p.Detalles)
                            .Where(p => p.Usuario == currentUser)
                            .ToList();
            }
            return View(pedidos);
        }

        // GET: Pedido/Details/5
        public ActionResult Details(int id)
        {
            var pedido = db.Pedidos
                           .Include(p => p.Detalles.Select(d => d.Producto))
                           .FirstOrDefault(p => p.PedidoId == id);

            if (pedido == null)
                return HttpNotFound();

            
            if (!User.IsInRole("Admin") && pedido.Usuario != User.Identity.Name)
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            return View(pedido);
        }
    }
}
