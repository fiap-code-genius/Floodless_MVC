using Floodless_MVC.Application.Interfaces;
using Floodless_MVC.Application.Services;
using Floodless_MVC.Domain.Interfaces;
using Floodless_MVC.Infrastructure.Data.AppData;
using Floodless_MVC.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;

// Configuração da licença do EPPlus
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);


//Descomentar esse trecho para usar com Docker com banco de dados em Container

var config = builder.Configuration;

// Tente obter as variáveis do ambiente
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

string connectionString;

if (!string.IsNullOrEmpty(dbHost) && !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
{
    // Ambiente Docker: substituir placeholders
    var connTemplate = config.GetConnectionString("OracleDocker");
    connectionString = connTemplate
        .Replace("${DB_HOST}", dbHost)
        .Replace("${DB_USER}", dbUser)
        .Replace("${DB_PASSWORD}", dbPassword);
}
else
{
    // Ambiente local: usa string fixa
    connectionString = config.GetConnectionString("OracleLocal");
}

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseOracle(connectionString));
 

//Utilizando Local(Comentar esse trecho caso use com Docker)
/*
builder.Services.AddDbContext<ApplicationContext>(x =>
{
    x.UseOracle(builder.Configuration.GetConnectionString("OracleLocal"));
});
*/

// Configurando as dependências necessárias
builder.Services.AddTransient<IRecursoRepository, RecursoRepository>();
builder.Services.AddTransient<IVoluntarioRepository, VoluntarioRepository>();
builder.Services.AddTransient<IVoluntarioApplication, VoluntarioApplication>();
builder.Services.AddTransient<IRecursoApplication, RecursoApplication>();
builder.Services.AddScoped<RelatorioExcelService>();

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options => {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

var app = builder.Build();



// Aplicar migrations automaticamente
// Descomentar ao utilizar o Container do banco de dados
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
