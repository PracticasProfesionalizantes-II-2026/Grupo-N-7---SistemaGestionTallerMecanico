using ClasesTallerMecanico.Datos;
using ClasesTallerMecanico.Endpoints;
using ClasesTallerMecanico.Logica;
using ClasesTallerMecanico.Repositorios;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<FacturasDBContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICategoriasTrabajosRepositorio, CategoriasTrabajosRepositorio>();
builder.Services.AddScoped<ICategoriasTrabajosLogica, CategoriasTrabajosLogica>();
builder.Services.AddScoped<IClientesRepositorio, ClientesRepositorio>();
builder.Services.AddScoped<IClientesLogica, ClientesLogica>();
builder.Services.AddScoped<IDetallesFacturasComprasRepositorio, DetalleFacturasComprasRepositorio>();
builder.Services.AddScoped<IDetallesFacturasComprasLogica, DetalleFacturasComprasLogica>();
builder.Services.AddScoped<IDetallesFacturasVentasRepositorio, DetallesFacturasVentasRepositorio>();
builder.Services.AddScoped<IDetallesFacturasVentasLogica, DetallesFacturasVentasLogica>();
builder.Services.AddScoped<IDetallesTurnosRepositorio, DetallesTurnosRepositorio>();
builder.Services.AddScoped<IDetallesTurnosLogica, DetallesTurnosLogica>();
builder.Services.AddScoped<IEstadosTurnoRepositorio, EstadosTurnoRepositorio>();
builder.Services.AddScoped<IEstadosTurnoLogica, EstadosTurnoLogica>();
builder.Services.AddScoped<IFacturasComprasRepositorio, FacturasComprasRepositorio>();
builder.Services.AddScoped<IFacturasComprasLogica, FacturasComprasLogica>();
builder.Services.AddScoped<IFacturasVentasRepositorio, FacturasVentasRepositorio>();
builder.Services.AddScoped<IFacturasVentasLogica, FacturasVentasLogica>();
builder.Services.AddScoped<IFormasPagoRepositorio, FormasPagoRepositorio>();
builder.Services.AddScoped<IFormasPagoLogica, FormasPagoLogica>();
builder.Services.AddScoped<IInsumosRepositorio, InsumosRepositorio>();
builder.Services.AddScoped<IInsumosLogica, InsumosLogica>();
builder.Services.AddScoped<IInsumosPorTrabajoRepositorio, InsumosPorTrabajoRepositorio>();
builder.Services.AddScoped<IInsumosPorTrabajoLogica, InsumosPorTrabajoLogica>();
builder.Services.AddScoped<ILocalidadesRepositorio, LocalidadesRepositorio>();
builder.Services.AddScoped<ILocalidadesLogica, LocalidadesLogica>();
builder.Services.AddScoped<IMaquinasRepositorio, MaquinasRepositorio>();
builder.Services.AddScoped<IMaquinasLogica, MaquinasLogica>();
builder.Services.AddScoped<IPersonasRepositorio, PersonasRepositorio>();
builder.Services.AddScoped<IPersonasLogica, PersonasLogica>();
builder.Services.AddScoped<IProveedoresRepositorio, ProveedoresRepositorio>();
builder.Services.AddScoped<IProveedoresLogica, ProveedoresLogica>();
builder.Services.AddScoped<IRolesRepositorio, RolesRepositorio>();
builder.Services.AddScoped<IRolesLogica, RolesLogica>();
builder.Services.AddScoped<ISesionesCajaRepositorio, SesionesCajaRepositorio>();
builder.Services.AddScoped<ISesionesCajaLogica, SesionesCajaLogica>();
builder.Services.AddScoped<ITiposTurnoRepositorio, TiposTurnoRepositorio>();
builder.Services.AddScoped<ITiposTurnoLogica, TiposTurnoLogica>();
builder.Services.AddScoped<ITrabajosRepositorio, TrabajosRepositorio>();
builder.Services.AddScoped<ITrabajosLogica, TrabajosLogica>();
builder.Services.AddScoped<ITrabajosPorTurnoRepositorio, TrabajosPorTurnoRepositorio>();
builder.Services.AddScoped<ITrabajosPorTurnoLogica, TrabajosPorTurnoLogica>();
builder.Services.AddScoped<ITurnosRepositorio, TurnosRepositorio>();
builder.Services.AddScoped<ITurnosLogica, TurnosLogica>();
builder.Services.AddScoped<IUsuariosRepositorio, UsuariosRepositorio>();
builder.Services.AddScoped<IUsuariosLogica, UsuariosLogica>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapCategoriasTrabajosEndpoints();
app.MapClientesEndpoints();
app.MapDetallesFacturasComprasEndpoints();
app.MapDetallesFacturasVentasEndpoints();
app.MapDetallesTurnosEndpoints();
app.MapEstadosTurnoEndpoints();
app.MapFacturasComprasEndpoints();
app.MapFacturasVentasEndpoints();
app.MapFormasPagoEndpoints();
app.MapInsumosEndpoints();
app.MapInsumosPorTrabajoEndpoints();
app.MapLocalidadesEndpoints();
app.MapMaquinasEndpoints();
app.MapPersonasEndpoints();
app.MapProveedoresEndpoints();
app.MapRolesEndpoints();
app.MapSesionesCajaEndpoints();
app.MapTiposTurnoEndpoints();
app.MapTrabajosEndpoints();
app.MapTrabajosPorTurnoEndpoints();
app.MapTurnosEndpoints();
app.MapUsuariosEndpoints();

app.Run();