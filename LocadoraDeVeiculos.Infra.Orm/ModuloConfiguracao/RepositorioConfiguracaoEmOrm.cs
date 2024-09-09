﻿using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracaoe;
using LocadoraDeVeiculos.Infra.Orm.Compartilhado;
using Microsoft.EntityFrameworkCore;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloConfiguracao;
public class RepositorioConfiguracaoEmOrm(LocadoraDeVeiculosDbContext _dbContext) : IRepositorioConfiguracao
{
    public void Inserir(Configuracao entidade)
    {
        ObterRegistros().Add(entidade);

        _dbContext.SaveChanges();
    }
    public void Editar(Configuracao entidade)
    {
        ObterRegistros().Update(entidade);

        _dbContext.SaveChanges();
    }

    protected DbSet<Configuracao> ObterRegistros() 
        => _dbContext.Configuracoes;

    public Configuracao? Selecionar(int usuarioId)
        => ObterRegistros().Where(c => c.UsuarioId == usuarioId).ToList().Count == 0 ? 
            null : ObterRegistros().Where(c => c.UsuarioId == usuarioId).ToList().First();
}