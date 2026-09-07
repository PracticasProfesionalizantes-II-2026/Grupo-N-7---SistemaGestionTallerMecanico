using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using TallerMecanicoFront.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBinderProviders.Insert(0, new FlexibleDecimalModelBinderProvider());

    // Política global: por defecto, TODA acción requiere usuario autenticado.
    // Las acciones que deben quedar públicas (Login, RecoverPassword) se marcan
    // explícitamente con [AllowAnonymous] en su controller.
    var politicaGlobal = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(politicaGlobal));
    options.Filters.Add<RestringirAccesoMecanicoFilter>();
});

builder.Services.AddHttpClient("TallerApi", client =>
{
    var baseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5237/";
    client.BaseAddress = new Uri(baseUrl);
});

// Autenticación basada en cookies. El navegador nunca ve ni administra
// credenciales por sí mismo: el servidor emite una cookie firmada y
// cifrada tras un login válido contra la API.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.Name = "TallerMecanico.Auth";
        options.Cookie.HttpOnly = true; // inaccesible desde JavaScript (mitiga XSS)
        options.Cookie.SameSite = SameSiteMode.Lax; // mitiga CSRF en navegación cruzada
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// El orden importa: primero se determina QUIÉN es (Authentication),
// recién después se decide QUÉ puede hacer (Authorization).
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuarios}/{action=Login}/{id?}")
    .WithStaticAssets();


app.Run();
