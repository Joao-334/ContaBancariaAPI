using ContaBancariaAPI.Data;
using ContaBancariaAPI.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContaBancariaAPI.Repository;

public class ContaBancariaRepository
{
    private readonly AppDbContext _context;

    public ContaBancariaRepository(AppDbContext context)
    {
        this._context = context;
    }
    public ContaBancaria? PegaContaBancariaPorNumeroDeConta(string numeroConta)
    {
        return this._context.Contas.FirstOrDefault(c => c.NumeroConta == numeroConta);
    }
    public void CriarContaBancaria(ContaBancaria conta)
    {
        _context.Contas.Add(conta);
        this._context.SaveChanges();
        return;
    }

    public void AtualizaContaBancaria(ContaBancaria conta)
    {
        var contaExiste = this.PegaContaBancariaPorId(conta.Id);

        if (contaExiste != null)
        {
            this._context.Update(conta);
            this._context.SaveChanges();
        }
    }
    private ContaBancaria? PegaContaBancariaPorId(int id)
    {
        return this._context.Contas.FirstOrDefault(conta => conta.Id == id);
    }
}