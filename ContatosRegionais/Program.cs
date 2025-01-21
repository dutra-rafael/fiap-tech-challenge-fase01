using ContatosRegionais.InjecaoDependencia;
using ContatosRegionais.Middlewares;
using ContatosRegionais.Persistencia.Contextos;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region [CONFIGURACOES DE RESPOSTA DA API]
builder.Services.AddControllers()
    .AddJsonOptions(option => option.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
    .AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddMemoryCache();
#endregion

#region [CONEXAO DE BANCO DE DADOS]
builder.Services.AddDbContext<Contexto>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")).EnableSensitiveDataLogging());
#endregion



#region [INJECAO DE DEPENDENCIA]
InversaoDeControle.ServicosRegistrados(builder.Services);
InversaoDeControle.RepositoriosRegistrados(builder.Services);
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddlewareLog();

app.UseAuthorization();

app.MapControllers();

app.Run();
