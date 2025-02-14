namespace TiendaVirtual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StockAlert : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.StockAlerts",
                c => new
                    {
                        StockAlertId = c.Int(nullable: false, identity: true),
                        ProductoId = c.Int(nullable: false),
                        Fecha = c.DateTime(nullable: false),
                        StockActual = c.Int(nullable: false),
                        Atendido = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.StockAlertId)
                .ForeignKey("dbo.Productoes", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.ProductoId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StockAlerts", "ProductoId", "dbo.Productoes");
            DropIndex("dbo.StockAlerts", new[] { "ProductoId" });
            DropTable("dbo.StockAlerts");
        }
    }
}
