using Luan.Data;
using Luan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDataContext>(options =>
    options.UseSqlite("Data Source=Leandro_Luan.db"));

var app = builder.Build();

app.MapGet("/", () => "API de Consumo de Água - Leandro & Luan");

// =================== Cadastro ===================
app.MapPost("/api/consumo/cadastrar",  ([FromServices] AppDataContext ctx, [FromBody] Consumo request) =>
{
    if (request.Mes < 1 || request.Mes > 12)
        return Results.BadRequest("Mês inválido.");

    if (request.Ano < 2000 || request.Ano > DateTime.Now.Year)
        return Results.BadRequest("Ano inválido.");

    var consumoExistente = ctx.Consumos
        .FirstOrDefault(c => c.Cpf == request.Cpf && c.Mes == request.Mes && c.Ano == request.Ano);

    if (consumoExistente != null)
        return Results.Conflict("Já existe uma leitura para este CPF neste mês e ano.");

    request.CalcularValores();

    ctx.Consumos.Add(request);
    ctx.SaveChanges();

    return Results.Created("/api/consumo/cadastrar", request);
});

// =================== listar ===================
app.MapGet("/api/consumo/listar", ([FromServices] AppDataContext ctx) =>
{

    if ( ctx.Consumos.Any())
    {
        return Results.Ok(ctx.Consumos.ToList());
    } return Results.BadRequest("Lista vazia");     
});


// =================== Busca ===================
app.MapGet("/api/consumo/buscar/{cpf}/{mes:int}/{ano:int}", ([FromServices] AppDataContext ctx, string cpf, int mes, int ano) =>
{
    var consumo = ctx.Consumos
        .FirstOrDefault(c => c.Cpf == cpf && c.Mes == mes && c.Ano == ano);
    if(consumo is not null)
    {
        return Results.Ok(consumo);
        
    } return Results.NotFound("Leitura não encontrada.");

});


// =================== remover ===================
app.MapDelete("/api/consumo/remover/{cpf}/{mes:int}/{ano:int}", ([FromServices] AppDataContext ctx, string cpf, int mes, int ano) =>
{
    var consumo = ctx.Consumos
        .FirstOrDefault(c => c.Cpf == cpf && c.Mes == mes && c.Ano == ano);

    if (consumo is null)
        return Results.NotFound("Leitura não encontrada.");

    ctx.Consumos.Remove(consumo);
    ctx.SaveChanges();
    return Results.Ok("Leitura removida com sucesso.");
});

// =================== Total Geral ===================
app.MapGet("/api/consumo/total-geral", ([FromServices] AppDataContext ctx) =>
{
    if (!ctx.Consumos.Any())
        return Results.NotFound("Nenhum registro encontrado.");

    var totalGeral = ctx.Consumos.Sum(c => c.Total);
    return Results.Ok(new{ totalGeral });
});

app.Run();
