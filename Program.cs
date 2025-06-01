using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Floodless_MVC.Infrastructure.Data.AppData;

var builder = WebApplication.CreateBuilder(args);

// Debug: Verificar o diretório atual
Console.WriteLine($"Diretório atual: {Directory.GetCurrentDirectory()}");

// Debug: Verificar se o arquivo existe
Console.WriteLine($"appsettings.json existe: {File.Exists("appsettings.json")}");
Console.WriteLine($"appsettings.json existe no diretório completo: {File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"))}");

var config = builder.Configuration;

// Debug: Listar todas as connection strings disponíveis
Console.WriteLine("Connection strings disponíveis:");
foreach (var conn in config.GetSection("ConnectionStrings").GetChildren())
{
    Console.WriteLine($"- {conn.Key}: {conn.Value}");
}

// Tente obter as variáveis do ambiente
var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbUser = Environment.GetEnvironmentVariable("DB_USER");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

string connectionString;

// Debug das variáveis de ambiente
Console.WriteLine($"DB_HOST: {dbHost}");
Console.WriteLine($"DB_USER: {dbUser}");
Console.WriteLine($"DB_PASSWORD: {dbPassword?.Length > 0}");

if (!string.IsNullOrEmpty(dbHost) && !string.IsNullOrEmpty(dbUser) && !string.IsNullOrEmpty(dbPassword))
{
    // Ambiente Docker: substituir placeholders
    var connTemplate = config.GetConnectionString("OracleDocker");
    Console.WriteLine("Usando template Docker: " + connTemplate);
    
    connectionString = connTemplate
        .Replace("${DB_HOST}", dbHost)
        .Replace("${DB_USER}", dbUser)
        .Replace("${DB_PASSWORD}", dbPassword);
}
else
{
    // Ambiente local: usa string fixa
    connectionString = config.GetConnectionString("OracleLocal");
    Console.WriteLine("Usando conexão local: " + connectionString);
}

Console.WriteLine("String de conexão final: " + connectionString);

// Debug: Tentar criar uma conexão de teste
try 
{
    using var connection = new Oracle.ManagedDataAccess.Client.OracleConnection(connectionString);
    connection.Open();
    Console.WriteLine("Conexão de teste bem sucedida!");
    connection.Close();
}
catch (Exception ex)
{
    Console.WriteLine($"Erro ao testar conexão: {ex.Message}");
    Console.WriteLine($"Stack trace: {ex.StackTrace}");
}

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseOracle(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run(); 