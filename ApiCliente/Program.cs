using ApiCliente.Context;
using ApiCliente.Entity;
using ApiCliente.Models;
using ApiCliente.Models.Request;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog.Sinks.MSSqlServer;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configurar Serilog para escribir logs en SQL Server
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties); // Opcional: eliminar propiedades extra

Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/api_log.txt", rollingInterval: RollingInterval.Day) // Logs en archivo
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        tableName: "Logs",
        autoCreateSqlTable: true, // Crea la tabla automáticamente si no existe
        columnOptions: columnOptions
    )
    .CreateLogger();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
