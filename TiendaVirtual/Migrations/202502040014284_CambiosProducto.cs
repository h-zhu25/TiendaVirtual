namespace TiendaVirtual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CambiosProducto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productoes", "Imagen", c => c.String());
            DropColumn("dbo.Productoes", "Imagen");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Productoes", "Imagen", c => c.String());
            DropColumn("dbo.Productoes", "Imagen");
        }
    }
}
