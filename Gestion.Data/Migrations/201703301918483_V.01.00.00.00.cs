namespace Gestion.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V01000000 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Audit.ChangeSets",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Comments = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                        Author_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.Author_Id, cascadeDelete: true)
                .Index(t => t.Author_Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 100),
                        Apellido = c.String(nullable: false, maxLength: 100),
                        Correo = c.String(nullable: false, maxLength: 255),
                        Telefono = c.String(),
                        NombreUsuario = c.String(nullable: false, maxLength: 100),
                        Password = c.String(),
                        ForzarCambioPassword = c.Boolean(nullable: false),
                        Activo = c.Boolean(nullable: false),
                        Estado = c.Int(nullable: false),
                        UsuarioIdCreacion = c.Long(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        UsuarioIdUltimaModificacion = c.Long(),
                        FechaUltimaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioIdCreacion)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioIdUltimaModificacion)
                .Index(t => t.NombreUsuario, unique: true, name: "UK_Usuarios_NombreUsuario")
                .Index(t => t.UsuarioIdCreacion)
                .Index(t => t.UsuarioIdUltimaModificacion);
            
            CreateTable(
                "dbo.Membresias",
                c => new
                    {
                        RolId = c.Long(nullable: false),
                        UsuarioId = c.Long(nullable: false),
                        VigenteDesde = c.DateTime(nullable: false),
                        VigenteHasta = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.RolId, t.UsuarioId })
                .ForeignKey("dbo.Roles", t => t.RolId, cascadeDelete: true)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioId, cascadeDelete: true)
                .Index(t => t.RolId)
                .Index(t => t.UsuarioId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Nombre = c.String(nullable: false, maxLength: 255),
                        Descripcion = c.String(maxLength: 500),
                        Estado = c.Int(nullable: false),
                        UsuarioIdCreacion = c.Long(nullable: false),
                        FechaCreacion = c.DateTime(nullable: false),
                        UsuarioIdUltimaModificacion = c.Long(),
                        FechaUltimaModificacion = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioIdCreacion)
                .ForeignKey("dbo.Usuarios", t => t.UsuarioIdUltimaModificacion)
                .Index(t => t.Nombre, unique: true, name: "UK_Roles_Nombre")
                .Index(t => t.UsuarioIdCreacion)
                .Index(t => t.UsuarioIdUltimaModificacion);
            
            CreateTable(
                "dbo.Permisos",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RecursoCodigo = c.String(nullable: false, maxLength: 20),
                        Accion = c.String(),
                        Descripcion = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Recursos", t => t.RecursoCodigo, cascadeDelete: true)
                .Index(t => t.RecursoCodigo);
            
            CreateTable(
                "dbo.Recursos",
                c => new
                    {
                        Codigo = c.String(nullable: false, maxLength: 20),
                        Descripcion = c.String(nullable: false, maxLength: 200),
                    })
                .PrimaryKey(t => t.Codigo);
            
            CreateTable(
                "Audit.ObjectChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TypeName = c.String(),
                        ObjectReference = c.String(),
                        ChangeSet_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Audit.ChangeSets", t => t.ChangeSet_Id, cascadeDelete: true)
                .Index(t => t.ChangeSet_Id);
            
            CreateTable(
                "Audit.PropertyChanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PropertyName = c.String(nullable: false, maxLength: 256),
                        Value = c.String(),
                        ValueAsInt = c.Int(),
                        ObjectChange_Id = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Audit.ObjectChanges", t => t.ObjectChange_Id, cascadeDelete: true)
                .Index(t => t.ObjectChange_Id);
            
            CreateTable(
                "dbo.RolesPermisos",
                c => new
                    {
                        RolId = c.Long(nullable: false),
                        PermisoId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.RolId, t.PermisoId })
                .ForeignKey("dbo.Roles", t => t.RolId, cascadeDelete: true)
                .ForeignKey("dbo.Permisos", t => t.PermisoId, cascadeDelete: true)
                .Index(t => t.RolId)
                .Index(t => t.PermisoId);

            Sql("INSERT INTO [dbo].[Usuarios] ([Nombre], [Apellido], [Correo], [NombreUsuario], [Estado], [FechaCreacion], [ForzarCambioPassword], [Activo], [Password], [UsuarioIdCreacion]) VALUES ('Administrador', 'Administrador', 'admin@gestion.biz', 'admin', 1, GETDATE(), 0, 1,'admin', 1)");

        }
        
        public override void Down()
        {
            DropForeignKey("Audit.ObjectChanges", "ChangeSet_Id", "Audit.ChangeSets");
            DropForeignKey("Audit.PropertyChanges", "ObjectChange_Id", "Audit.ObjectChanges");
            DropForeignKey("Audit.ChangeSets", "Author_Id", "dbo.Usuarios");
            DropForeignKey("dbo.Usuarios", "UsuarioIdUltimaModificacion", "dbo.Usuarios");
            DropForeignKey("dbo.Usuarios", "UsuarioIdCreacion", "dbo.Usuarios");
            DropForeignKey("dbo.Membresias", "UsuarioId", "dbo.Usuarios");
            DropForeignKey("dbo.Membresias", "RolId", "dbo.Roles");
            DropForeignKey("dbo.Roles", "UsuarioIdUltimaModificacion", "dbo.Usuarios");
            DropForeignKey("dbo.Roles", "UsuarioIdCreacion", "dbo.Usuarios");
            DropForeignKey("dbo.RolesPermisos", "PermisoId", "dbo.Permisos");
            DropForeignKey("dbo.RolesPermisos", "RolId", "dbo.Roles");
            DropForeignKey("dbo.Permisos", "RecursoCodigo", "dbo.Recursos");
            DropIndex("dbo.RolesPermisos", new[] { "PermisoId" });
            DropIndex("dbo.RolesPermisos", new[] { "RolId" });
            DropIndex("Audit.PropertyChanges", new[] { "ObjectChange_Id" });
            DropIndex("Audit.ObjectChanges", new[] { "ChangeSet_Id" });
            DropIndex("dbo.Permisos", new[] { "RecursoCodigo" });
            DropIndex("dbo.Roles", new[] { "UsuarioIdUltimaModificacion" });
            DropIndex("dbo.Roles", new[] { "UsuarioIdCreacion" });
            DropIndex("dbo.Roles", "UK_Roles_Nombre");
            DropIndex("dbo.Membresias", new[] { "UsuarioId" });
            DropIndex("dbo.Membresias", new[] { "RolId" });
            DropIndex("dbo.Usuarios", new[] { "UsuarioIdUltimaModificacion" });
            DropIndex("dbo.Usuarios", new[] { "UsuarioIdCreacion" });
            DropIndex("dbo.Usuarios", "UK_Usuarios_NombreUsuario");
            DropIndex("Audit.ChangeSets", new[] { "Author_Id" });
            DropTable("dbo.RolesPermisos");
            DropTable("Audit.PropertyChanges");
            DropTable("Audit.ObjectChanges");
            DropTable("dbo.Recursos");
            DropTable("dbo.Permisos");
            DropTable("dbo.Roles");
            DropTable("dbo.Membresias");
            DropTable("dbo.Usuarios");
            DropTable("Audit.ChangeSets");
        }
    }
}
