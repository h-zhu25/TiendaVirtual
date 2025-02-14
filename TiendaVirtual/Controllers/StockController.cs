using System.Linq;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StockController : Controller
    {
        private TiendaContext db = new TiendaContext();

        // GET: Stock
        public ActionResult Index()
        {
            
            var alerts = db.StockAlerts.Include("Producto")
                                        .Where(a => !a.Atendido)
                                        .ToList();
            return View(alerts);
        }

        
        public ActionResult MarkAsAttended(int id)
        {
            var alert = db.StockAlerts.Find(id);
            if (alert != null)
            {
                alert.Atendido = true;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
