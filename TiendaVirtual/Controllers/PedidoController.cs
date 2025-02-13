using System;
using System.Collections.Generic;
using System.Data.Entity; 
using System.Linq;
using System.Web.Mvc; 
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    
    public class PedidoController : Controller
    {
        private TiendaContext db = new TiendaContext(); 

        // GET: Pedido
        public ActionResult Index()
        {
            var pedidos = db.Pedidos.Include(p => p.Detalles).ToList(); 
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

            return View(pedido); 
        }
    }
}