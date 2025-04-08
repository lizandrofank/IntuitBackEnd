using ApiCliente.Entity;
using ApiCliente.Models.Request;
using ApiCliente.Models.Response;
using Microsoft.AspNetCore.Mvc;

namespace ApiCliente.Context
{
    public interface IClienteRepository
    {
        Task<IEnumerable<ClienteResponse>> GetAll();
        Task<ClienteResponse> Get(int id);
        Task<IEnumerable<ClienteResponse>> Search(string nombre);
        Task <ClienteResponse> AddClient([FromBody] ClienteRequest request);
        Task<(bool Modify, ClienteResponse)> Update(int id, [FromBody] ClienteRequest request);
    }
}
