using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class Carrito
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioUnitario { get; set; }
        public int Cantidad { get; set; }

        // Propiedad calculada:
        public decimal Subtotal
        {
            get { return PrecioUnitario * Cantidad; }
        }
    }
}