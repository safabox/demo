using System;
using Gestion.Common.Validators;

namespace Gestion.API.Models.Membresias
{
    public class PostMembresiaModel
    {
        public long RolId { get; set; }
        public DateTime VigenteDesde { get; set; }

        [NullableDateTimeRangeValidation("VigenteDesde", ErrorMessage = "Rango de fechas inválido")]
        public Nullable<DateTime> VigenteHasta { get; set; }
    }
}