using Gestion.Data.Audit;
using Gestion.Common.Domain.Seguridad;
using Gestion.Common.Domain.Audit;
using Gestion.Common.Data;
using Gestion.Common.Data.Seguridad;
using Gestion.Common.Data.Audit;
using FrameLog;
using FrameLog.History;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Gestion.Data
{
    public class GestionDbContext : DbContext, IPermisoDataContext, IUnitOfWork, IAuditDataContext, IRolDataContext
    {
        private readonly FrameLogModule<ChangeSet, Usuario> logger;

        private HistoryExplorer<ChangeSet, Usuario> _historyExplorer;

        public static void UpdateDatabase()
        {
            using (var context = new GestionDbContext())
            {
                Database.SetInitializer(new MigrateDatabaseToLatestVersion<GestionDbContext, Migrations.Configuration>());
                context.Database.Initialize(true);
            }
        }

        public GestionDbContext()
            : this("GestionContext")
        {
        }

        public GestionDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
#if DEBUG
            this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
#endif

            this.logger = new FrameLogModule<ChangeSet, Usuario>(new ChangeSetFactory(), new GestionDbContextAuditAdapter(this));
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Carga todos los mappers directamente desde el assembly
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Usuario> Usuarios { get; set; }
        public virtual DbSet<Permiso> Permisos { get; set; }
        public virtual DbSet<Rol> Roles { get; set; }
        public virtual DbSet<Recurso> Recursos { get; set; }
        public virtual DbSet<Membresia> Membresias { get; set; }
        public virtual DbSet<ChangeSet> ChangeSets { get; set; }
        public virtual DbSet<ObjectChange> ObjectChanges { get; set; }
        public virtual DbSet<PropertyChange> PropertyChanges { get; set; }

        public HistoryExplorer<ChangeSet, Usuario> HistoryExplorer
        {
            get
            {
                if (_historyExplorer == null)
                {
                    _historyExplorer = new HistoryExplorer<ChangeSet, Usuario>(new GestionDbContextAuditAdapter(this), null, HistoryExplorerCloneStrategies.DeepCopy);
                }
                return _historyExplorer;
            }
        }

        public Usuario GetUsuarioByNombreUsuario(string nombreUsuario)
        {
            return Usuarios.First(x => x.NombreUsuario == nombreUsuario);
        }

        public void SetEntityState<T>(T entry, EntityState state) where T : class
        {
            this.Entry<T>(entry).State = state;
        }

        public void Reload<T>(T entry) where T : class
        {
            this.Entry<T>(entry).Reload();
        }

        public Task<int> SaveChangesAsync(Usuario user, string comments = null)
        {
            return this.SaveChangesAsync(CancellationToken.None, user, comments);

        }

        public Task<int> SaveChangesAsync(string userName, string comments = null)
        {
            return this.SaveChangesAsync(GetUsuarioByNombreUsuario(userName), comments);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken, string userName, string comments = null)
        {
            return this.SaveChangesAsync(cancellationToken, GetUsuarioByNombreUsuario(userName), comments);
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken, Usuario user, string comments = null)
        {

            var result = await logger.SaveChangesAsync(user, cancellationToken);
            if (result.AffectedObjectCount > 0 && !String.IsNullOrEmpty(comments))
            {
                if (result.ChangeSet != null)
                {
                    result.ChangeSet.Comments = comments;
                    base.SaveChanges();
                }
            }
            return result.AffectedObjectCount;
        }

        public int SaveChanges(Usuario user, string comments = null)
        {
            var result = logger.SaveChanges(user);
            if (result.AffectedObjectCount > 0 && !String.IsNullOrEmpty(comments))
            {
                if (result.ChangeSet != null)
                {
                    result.ChangeSet.Comments = comments;
                    base.SaveChanges();
                }
            }
            return result.AffectedObjectCount;
        }

    }
}
