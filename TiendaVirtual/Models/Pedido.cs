using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Usuario { get; set; }
                                            
        public decimal Total { get; set; }

        // Relación con DetallePedido
        public virtual ICollection<DetallePedido> Detalles { get; set; }
    }
}