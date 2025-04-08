using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Unipluss.Sign.Client.Code;
using Unipluss.Sign.Common.Rest.URLs;

namespace ApiCliente.Entity
{
    public class Cliente
     { 
        [Key]
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;      
        public DateTime? FechaNacimiento { get; set; }
        public string? Cuit { get; set; } = string.Empty;
        public string Domicilio { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;
    }
   
}
