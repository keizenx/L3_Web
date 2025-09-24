using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using IITWebApp.Data;
using IITWebApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configuration de la base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Enregistrer le service de génération de matricules
builder.Services.AddScoped<IMatriculeService, MatriculeService>();

var app = builder.Build();

// DÉSACTIVER COMPLÈTEMENT LA CSP - PLACER EN PREMIER, AVANT TOUT
app.Use(async (context, next) =>
{
    // Supprimer toutes les headers CSP possibles
    context.Response.Headers.Remove("Content-Security-Policy");
    context.Response.Headers.Remove("Content-Security-Policy-Report-Only");
    context.Response.Headers.Remove("X-Content-Security-Policy");
    context.Response.Headers.Remove("X-Frame-Options");
    context.Response.Headers.Remove("X-XSS-Protection");
    await next();
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware pour logger toutes les requêtes
app.Use(async (context, next) =>
{
    Console.WriteLine($"[DEBUG] Request: {context.Request.Method} {context.Request.Path}");
    try
    {
        await next();
        Console.WriteLine($"[DEBUG] Response: {context.Response.StatusCode}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"[ERROR] Exception in middleware: {ex.Message}");
        Console.WriteLine($"[ERROR] Stack trace: {ex.StackTrace}");
        throw;
    }
});

// Configuration des fichiers statiques
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
