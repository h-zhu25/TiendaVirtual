namespace TiendaVirtual.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProducto : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Productoes", "ImagenUrl", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Productoes", "ImagenUrl");
        }
    }
}
