using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TiendaVirtual.Models;

namespace TiendaVirtual.Controllers
{
    public class CarritoController : Controller
    {
        private const string CART_SESSION_KEY = "Carrito";
        private TiendaContext db = new TiendaContext();

        // Auxiliar: obtiene o crea la lista de Carrito en Session
        private List<Carrito> GetCarrito()
        {
            var carrito = Session[CART_SESSION_KEY] as List<Carrito>;
            if (carrito == null)
            {
                carrito = new List<Carrito>();
                Session[CART_SESSION_KEY] = carrito;
            }
            return carrito;
        }

        // GET: Carrito (muestra la vista Index)
        public ActionResult Index()
        {
            var carrito = GetCarrito();
            return View(carrito);
        }

        /// <summary>
        /// Agrega un producto al carrito, cantidad fija = 1 (GET)
        /// </summary>
        public ActionResult Add(int id)
        {
            var producto = db.Productos.Find(id);
            if (producto != null && producto.Stock > 0)
            {
                var carrito = GetCarrito();
                var item = carrito.FirstOrDefault(c => c.ProductoId == producto.ProductoId);
                if (item == null)
                {
                    carrito.Add(new Carrito
                    {
                        ProductoId = producto.ProductoId,
                        Nombre = producto.Nombre,
                        PrecioUnitario = producto.Precio,
                        Cantidad = 1
                    });
                }
                else
                {
                    if (item.Cantidad < producto.Stock) 
                    {
                        item.Cantidad++;
                    }
                    else
                    {
                        TempData["Error"] = "No hay suficiente stock disponible.";
                    }
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Agrega producto al carrito con cantidad elegida por el usuario (POST)
        /// </summary>
        [HttpPost]
        public ActionResult AddConCantidad(int productoId, int cantidad)
        {
            if (cantidad < 1) return RedirectToAction("Index");

            var producto = db.Productos.Find(productoId);
            if (producto != null && producto.Stock >= cantidad)
            {
                var carrito = GetCarrito();
                var item = carrito.FirstOrDefault(c => c.ProductoId == productoId);
                if (item == null)
                {
                    carrito.Add(new Carrito
                    {
                        ProductoId = producto.ProductoId,
                        Nombre = producto.Nombre,
                        PrecioUnitario = producto.Precio,
                        Cantidad = cantidad
                    });
                }
                else
                {
                    item.Cantidad += cantidad;
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Elimina todo un ítem del carrito (Remove)
        /// </summary>
        public ActionResult Remove(int id)
        {
            var carrito = GetCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);
            if (item != null)
            {
                carrito.Remove(item);
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Quita parte de la cantidad de un ítem. Si quita >= Cantidad actual, elimina el ítem.
        /// </summary>
        [HttpPost]
        public ActionResult RemovePartial(int productoId, int cantidad)
        {
            if (cantidad < 1)
                return RedirectToAction("Index");

            var carrito = GetCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == productoId);
            if (item != null)
            {
                if (cantidad >= item.Cantidad)
                {
                    // Si se quita tanto o más que la cantidad actual, eliminamos todo
                    carrito.Remove(item);
                }
                else
                {
                    // Solo quitamos una parte
                    item.Cantidad -= cantidad;
                }
            }
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Vacía todo el carrito
        /// </summary>
        public ActionResult Clear()
        {
            var carrito = GetCarrito();
            carrito.Clear();
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

        public ActionResult ConfirmarPedido()
        {
            var carrito = GetCarrito();
            if (carrito == null || !carrito.Any())
            {
                TempData["Error"] = "El carrito está vacío.";
                return RedirectToAction("Index");
            }

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // 1) Crear nuevo pedido
                    var pedido = new Pedido
                    {
                        Fecha = DateTime.Now,
                        Usuario = User.Identity.Name, // 如果有用户系统
                        Total = carrito.Sum(c => c.PrecioUnitario * c.Cantidad)
                    };
                    db.Pedidos.Add(pedido);
                    db.SaveChanges();

                    // 2) Guardar los detalles del pedido y reducir el stock
                    foreach (var item in carrito)
                    {
                        
                        var detalle = new DetallePedido
                        {
                            PedidoId = pedido.PedidoId,
                            ProductoId = item.ProductoId,
                            Cantidad = item.Cantidad,
                            Subtotal = item.Cantidad * item.PrecioUnitario
                        };
                        db.DetallesPedido.Add(detalle);

                        var producto = db.Productos.Find(item.ProductoId);
                        if (producto != null)
                        {
                            if (producto.Stock < item.Cantidad)
                            {
                                TempData["Error"] = $"Stock insuficiente para {producto.Nombre}.";
                                transaction.Rollback();
                                return RedirectToAction("Index");
                            }

                            producto.Stock -= item.Cantidad;

                            // 如果库存减少到 0，可以选择设置“已售罄”状态
                            if (producto.Stock == 0)
                            {
                                // Opcional: 标记为“已售罄”
                                // producto.Disponible = false; // 如果有字段
                            }
                        }
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    // 3) 清空购物车
                    carrito.Clear();
                    Session[CART_SESSION_KEY] = carrito;

                    TempData["Success"] = "Pedido confirmado con éxito.";
                    return RedirectToAction("Index", "Producto"); // 或跳转到“订单详情”页面
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    TempData["Error"] = $"Ocurrió un error: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }
        }

    }
}
