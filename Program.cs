using ContaBancariaAPI;
using ContaBancariaAPI.Data;
using ContaBancariaAPI.Middlewares;
using ContaBancariaAPI.Records;
using ContaBancariaAPI.Repository;
using ContaBancariaAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(connection));
builder.Services.AddScoped<ContaBancariaRepository>();
builder.Services.AddScoped<ContaBancariaService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!dbContext.Database.CanConnect())
    {
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
}

app.UseMiddleware<TratamentoDeExcecoes>();

app.MapPost("/contas", (ContaBancaria contaBancaria, ContaBancariaService contaBancariaService) =>
    {
        contaBancariaService.VerificaSeContaExisteEAdicionaNovaContaERetorna(contaBancaria);
        return Results.Json
        (
            new
            {
                Message = "Conta criada com sucesso!",
                Conta = contaBancaria
            }
        );
    }
);

app.MapPost("/contas/{numeroConta}/{operacao}", (string numeroConta, string operacao, Transacao transacao, ContaBancariaService contaBancariaService) =>
    {
        ContaBancaria conta = contaBancariaService.VerificaSeContaExisteEAtualizaSeuSaldoERetorna(numeroConta, operacao, transacao)!;
        return Results.Json(new
        {
            Message = $"{operacao.First().ToString().ToUpper() + operacao.Substring(1)} de R${transacao.Valor} realizado com sucesso!",
            Conta = new ContaBancariaDTO(conta.Titular, conta.NumeroConta, conta.Saldo)
        });
    }
);

app.MapGet("/contas/{numeroConta}/saldo", (string numeroConta, ContaBancariaService contaBancariaService) =>
    {
        ContaBancaria conta = contaBancariaService.VerificaSeContaExisteERetorna(numeroConta)!;

        return Results.Json(new
            {
                Message = $"O saldo da conta é de R${conta.Saldo}",
                Conta = new ContaBancariaDTO(conta.Titular, conta.NumeroConta, conta.Saldo)
            }
        );

    }
);

app.Run();