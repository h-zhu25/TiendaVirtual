using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TiendaVirtual.Models
{
    public class StockAlert
    {
        public int StockAlertId { get; set; }     
        public int ProductoId { get; set; }        
        public DateTime Fecha { get; set; }         
        public int StockActual { get; set; }        
        public bool Atendido { get; set; }          

        public virtual Producto Producto { get; set; }
    }
}