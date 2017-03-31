using System;

namespace Gestion.API.Models.Membresias
{
    public class MembresiaModel : Model
    {
        public long RolId { get; set; }
        public string RolNombre { get; set; }
        public DateTime VigenteDesde { get; set; }
        public Nullable<DateTime> VigenteHasta { get; set; }
    }
}