namespace TiendaVirtual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDetallePedido : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DetallePedidoes", "Subtotal", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DetallePedidoes", "Subtotal");
        }
    }
}
