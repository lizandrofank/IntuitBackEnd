using ApiCliente.Context;
using ApiCliente.Entity;
using ApiCliente.Models;
using ApiCliente.Models.Request;
using ApiCliente.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiCliente.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteController(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        /// <summary>
        /// Metodo para obtener un listado de todos los clientes.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll() {
            try
            {
                var clientes = await _clienteRepository.GetAll();
                return StatusCode(200,clientes);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al obtener la lista de clientes.");
                return StatusCode(500, "Error interno del servidor.");
            }
        }
        /// <summary>
        /// Metodo para obtener un cliente especifico mediante su id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var cliente = await _clienteRepository.Get(id);
                if (cliente is null)
                {
                    Log.Warning($"Cliente con Id {id} no encontrado.", id);
                    return NotFound($"Cliente con Id {id} no encontrado.");
                }

                return StatusCode(200,cliente);
            }
            catch (Exception ex)
            {
                Log.Error($"Cliente con Id {id} no encontrado.", id);
                return StatusCode(500, "Error interno del servidor.");
            }

        }
        /// <summary>
        /// Metodo para buscar clientes por nombre.
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpGet("Search/{nombre}")]
        public async Task<IActionResult> Search(string nombre)
        {
            try
            {
                var clientes = await _clienteRepository.Search(nombre);
                if (!clientes.Any())
                {
                    Log.Warning($"No se a encontrado un cliente con el nombre {nombre} ingreasado.\"", nombre);
                    return NotFound("No se a encontrado un cliente con el nombre ingreasado.");
                }     
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                Log.Error($"Cliente con nombre {nombre} no encontrado.", nombre);
                return StatusCode(500, "Error interno del servidor.");
            }
           
        }
        /// <summary>
        /// Metodo para registrar un nuevo Cliente.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Insert")]
        public async Task<IActionResult> AddClient([FromBody] ClienteRequest request)
        {
            try
            {
                var validator = new ClienteValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    var errores = result.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new { Mensaje = "Errores de validación", Errores = errores });
                }

                var cliente = await _clienteRepository.AddClient(request);
                
                if (cliente is null) 
                {
                    Log.Error($"Error al registrar un nuevo cliente: {request.Nombre} {request.Apellido}.", request.Nombre, request.Apellido);
                    return BadRequest("Error al registrar el nuevo Cliente.");
                }
                    
                Log.Information($"Se registro correctamente un nuevo cliente: {request.Nombre} {request.Apellido}.", request.Nombre, request.Apellido);
                return StatusCode(201,cliente);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al registrar un nuevo cliente: {request.Nombre} {request.Apellido}.", request.Nombre,request.Apellido);
                return StatusCode(500, "Error interno del servidor.");
            }
           

        }

        /// <summary>
        /// Metodo para actualizar un cliente mediante su id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteRequest request)
        {
            try
            {
                var validator = new ClienteValidator();
                var result = validator.Validate(request);

                if (!result.IsValid)
                {
                    var errores = result.Errors.Select(e => e.ErrorMessage).ToList();
                    return BadRequest(new { Mensaje = "Errores de validación", Errores = errores });
                }

                var (modify,cliente) = await _clienteRepository.Update(id, request);

                if (cliente is null)
                {
                    Log.Warning($"Cliente con Id {id} no encontrado.", id);
                    return NotFound($"No se encontro el cliente con Id {id}");
                }
                if (!modify)
                    return NoContent();

                Log.Information($"Se actualizo correctamente el cliente: {request.Nombre} {request.Apellido}.", request.Nombre, request.Apellido);
                return Ok(cliente);
            }
            catch (Exception ex)
            {
                Log.Error($"Error al actualizar el cliente: {request.Nombre} {request.Apellido}.", request.Nombre, request.Apellido);
                return StatusCode(500, "Error interno del servidor.");
            }
            
        }
    }
}
