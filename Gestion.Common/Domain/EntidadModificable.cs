using System;
using Gestion.Common.Domain.Seguridad;
using FrameLog.Filter;

namespace Gestion.Common.Domain
{
    [Serializable]
    public abstract class EntidadModificable : Entidad
    {
        public EntidadModificable()
        {
            this.Estado = EstadosEntidad.Activa;
            this.FechaCreacion = DateTime.Now;
        }

        public EstadosEntidad Estado { get; set; }

        [DoNotLog]
        public virtual Usuario UsuarioCreacion { get; set; }

        [DoNotLog]
        public long UsuarioIdCreacion { get; set; }

        [DoNotLog]
        public DateTime FechaCreacion { get; set; }

        [DoNotLog]
        public virtual Usuario UsuarioUltimaModificacion { get; set; }

        [DoNotLog]
        public Nullable<long> UsuarioIdUltimaModificacion { get; set; }

        [DoNotLog]
        public Nullable<DateTime> FechaUltimaModificacion { get; set; }

        public void Eliminar(Usuario usuario)
        {
            this.Estado = EstadosEntidad.Eliminada;
            this.FechaUltimaModificacion = DateTime.Now;
            this.UsuarioUltimaModificacion = usuario;
            this.UsuarioIdUltimaModificacion = usuario.Id;
        }

        public void Recuperar(Usuario usuario)
        {
            this.Estado = EstadosEntidad.Activa;
            this.FechaUltimaModificacion = DateTime.Now;
            this.UsuarioUltimaModificacion = usuario;
            this.UsuarioIdUltimaModificacion = usuario.Id;
        }
    }
}
