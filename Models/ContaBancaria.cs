using ContaBancariaAPI.Exceptions;
using System.Text.Json.Serialization;
public class ContaBancaria
{
    public int Id { get; set; }
    public required string Titular { get; set; }
    public required string NumeroConta { get; set; }
    [JsonPropertyName(name: "saldoInicial")]
    public required decimal Saldo { get; set; }
    public void Sacar(decimal valor)
    {
        if (valor <= 0 || Saldo < valor)
        {
            throw new ApiException(400,"Saldo insuficiente ou valor inválido!");
        }

        Saldo -= valor;
    }

    public void Depositar(decimal valor)
    {
        if (valor <= 0)
        {
            throw new ApiException(400, "Saldo insuficiente ou valor inválido!");
        }

        Saldo += valor;
    }
}
