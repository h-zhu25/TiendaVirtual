using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TiendaVirtual.Models
{

    public class TiendaContext : IdentityDbContext<ApplicationUser>
    {
        // Este "TiendaContext" debe coincidir con la connectionString del Web.config
        public TiendaContext() : base("TiendaContext", throwIfV1Schema: false)
        {
        }

        // Definir las tablas de la base de datos (entidades)
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<StockAlert> StockAlerts { get; set; }


        // Si necesitas más tablas, agrégalas aquí...

        // Método para crear el contexto (requerido por Identity)
        public static TiendaContext Create()
        {
            return new TiendaContext();
        }
    }
}

