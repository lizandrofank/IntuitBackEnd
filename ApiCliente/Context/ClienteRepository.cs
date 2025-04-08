using ApiCliente.Entity;
using ApiCliente.Models.Request;
using ApiCliente.Models.Response;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;


namespace ApiCliente.Context
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Metodo para obtener un cliente especifico mediante su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ClienteResponse> Get(int id)
        {
      
                var cliente = await _context.Cliente.FindAsync(id);
                
                if (cliente is null)
                    return  null;
                
                return new ClienteResponse
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Apellido = cliente.Apellido,
                    FechaNacimiento = cliente.FechaNacimiento,
                    Cuit = cliente.Cuit,
                    Domicilio = cliente.Domicilio,
                    Telefono = cliente.Telefono,
                    Email = cliente.Email
                };

        }
      
        /// <summary>
        /// Metodo para obtener un listado de todos los clientes.
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<ClienteResponse>> GetAll()
        {
          return await _context.Cliente.Select(
               c => new ClienteResponse
               {
                   Id = c.Id,
                   Nombre = c.Nombre,
                   Apellido = c.Apellido,
                   FechaNacimiento = c.FechaNacimiento,
                   Cuit = c.Cuit,
                   Domicilio = c.Domicilio,
                   Telefono = c.Telefono,
                   Email = c.Email
               }
               ).ToListAsync();
        }
        /// <summary>
        /// Metodo para buscar clientes por nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ClienteResponse>> Search(string nombre)
        {
            var clientes = await _context.Cliente.Where(c => c.Nombre.ToUpper().Contains(nombre.ToUpper())).ToListAsync();
            return clientes.Select(c => (ClienteResponse)c);
        }
        /// <summary>
        /// Metodo para registrar un nuevo Cliente.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ClienteResponse> AddClient([FromBody] ClienteRequest request)
        {
            var cliente = new Cliente
            {

                Nombre = request.Nombre,
                Apellido = request.Apellido,
                FechaNacimiento = request.FechaNacimiento,
                Cuit = request.Cuit,
                Domicilio = request.Domicilio,
                Telefono = request.Telefono,
                Email = request.Email,
            };
            try
            {
                await _context.Cliente.AddAsync(cliente);
                await _context.SaveChangesAsync();
                return (ClienteResponse)cliente;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Metodo para actualizar un cliente mediante su id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<(bool Modify,ClienteResponse)> Update(int id, [FromBody] ClienteRequest request)
        {
            var cliente = await _context.Cliente.FindAsync(id);

            if (cliente is null)
                return (false,null);

            cliente.Nombre = request.Nombre;
            cliente.Apellido = request.Apellido;
            cliente.FechaNacimiento = request.FechaNacimiento;
            cliente.Cuit = request.Cuit;
            cliente.Domicilio = request.Domicilio;
            cliente.Telefono = request.Telefono;
            cliente.Email = request.Email;

            var entry = _context.Entry(cliente);

            foreach (var p in entry.Properties)
            {
                if (!Equals(p.OriginalValue, p.CurrentValue))
                {
                    p.IsModified = true;
                    break;
                }
            }   
            
            if  (!entry.Properties.Any(p => p.IsModified))
                return (false,(ClienteResponse)cliente);

            _context.Cliente.Update(cliente);
            await _context.SaveChangesAsync();

            return (true,(ClienteResponse)cliente);
        }

    }
}
