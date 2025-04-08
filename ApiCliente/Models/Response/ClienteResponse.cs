using ApiCliente.Entity;

namespace ApiCliente.Models.Response
{
    public class ClienteResponse
    {
        public int Id { get; set; }
        public string? Nombre { get; set; } = string.Empty; 
        public string? Apellido { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public string? Cuit { get; set; } = string.Empty;
        public string? Domicilio { get; set; } = string.Empty;
        public string? Telefono { get; set; } = string.Empty;
        public string? Email { get; set; } = string.Empty;

        public static explicit operator ClienteResponse(Cliente c)
        {
            return new ClienteResponse
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Apellido = c.Apellido,
                FechaNacimiento = c.FechaNacimiento,
                Cuit = c.Cuit,
                Domicilio = c.Domicilio,
                Telefono = c.Telefono,
                Email = c.Email,
            };
        }
    }
}
