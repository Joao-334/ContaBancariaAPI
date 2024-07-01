using ContaBancariaAPI.Exceptions;
using ContaBancariaAPI.Records;
using ContaBancariaAPI.Repository;
using System.Drawing;

namespace ContaBancariaAPI.Services;
public class ContaBancariaService
{
    private readonly ContaBancariaRepository _repository;

    private readonly Dictionary<string, Action<ContaBancaria, decimal>> _operacoes;

    public ContaBancariaService(ContaBancariaRepository repository)
    {
        _repository = repository;
        _operacoes = new Dictionary<string, Action<ContaBancaria, decimal>>(StringComparer.OrdinalIgnoreCase)
        {
            { "depositar", (conta, valor) => conta.Depositar(valor) },
            { "sacar", (conta, valor) => conta.Sacar(valor) }
        };
    }

    public ContaBancaria? VerificaSeContaExisteERetorna(string numeroConta)
    {
        ContaBancaria conta = this._repository.PegaContaBancariaPorNumeroDeConta(numeroConta)!;

        return conta ?? throw new ApiException(404, "Conta não encontrada");

    }

    public ContaBancaria? VerificaSeContaExisteEAdicionaNovaContaERetorna(ContaBancaria conta)
    {
        var contaExiste = this._repository.PegaContaBancariaPorNumeroDeConta(conta.NumeroConta) ?? throw new ApiException(404, "Conta já existe!"); ;

        this._repository.CriarContaBancaria(conta);
        return conta;
    }

    public ContaBancaria? VerificaSeContaExisteEAtualizaSeuSaldoERetorna(string numeroConta, string operacao, Transacao transacao)
    {
        ContaBancaria conta = this._repository.PegaContaBancariaPorNumeroDeConta(numeroConta)
                ?? throw new ApiException(404, "Conta não existe!");

        if (!_operacoes.TryGetValue(operacao, out var acao))
        {
            throw new ApiException(400, "Operação não suportada!");
        }

        acao(conta, transacao.Valor);
        this. _repository.AtualizaContaBancaria(conta);

        return conta;
    }
}
