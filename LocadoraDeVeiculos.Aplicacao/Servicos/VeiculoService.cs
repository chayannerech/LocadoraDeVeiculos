﻿using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class VeiculoService(IRepositorioVeiculo repositorioVeiculos, IRepositorioGrupoDeAutomoveis repositorioGrupo)
{
    public Result<Veiculo> Inserir(Veiculo registro, int grupoId)
    {
        var grupoSelecionado = repositorioGrupo
            .SelecionarPorId(grupoId);

        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");

        registro.GrupoDeAutomoveis = grupoSelecionado;

        var erros = registro.Validar();
        if (erros.Count != 0)
            return Result.Fail(erros[0]);

        repositorioVeiculos.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<Veiculo> Editar(Veiculo registroAtualizado, int grupoId)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroAtualizado.Id);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        var grupoSelecionado = repositorioGrupo.SelecionarPorId(grupoId);

        if (grupoSelecionado is null)
            return Result.Fail("O grupo não foi selecionado!");

        registro.GrupoDeAutomoveis = grupoSelecionado;
        registro.Placa = registroAtualizado.Placa;
        registro.Marca = registroAtualizado.Marca;
        registro.Cor = registroAtualizado.Cor;
        registro.Modelo = registroAtualizado.Modelo;
        registro.TipoCombustivel = registroAtualizado.TipoCombustivel;
        registro.CapacidadeCombustivel = registroAtualizado.CapacidadeCombustivel;
        registro.Ano = registroAtualizado.Ano;
        registro.ImagemEmBytes = registroAtualizado.ImagemEmBytes;
        registro.TipoDaImagem = registroAtualizado.TipoDaImagem;

        var erros = registro.Validar();
            if (erros.Count != 0)
                return Result.Fail(erros[0]);

        repositorioVeiculos.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        repositorioVeiculos.Excluir(registro);

        return Result.Ok();
    }

    public Result<Veiculo> SelecionarPorId(int registroId)
    {
        var registro = repositorioVeiculos.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Veiculo>> SelecionarTodos(int usuarioId)
    {
        var registros = repositorioVeiculos
            .Filtrar(f => f.UsuarioId == usuarioId);

        return Result.Ok(registros);
    }

    public Result<List<IGrouping<string, Veiculo>>> ObterVeiculosAgrupadosPorGrupo(int? usuarioId = null)
    {
        if (usuarioId is not null)
            return Result.Ok(repositorioVeiculos.ObterVeiculosAgrupadosPorGrupo(usuarioId.Value));

        return Result.Ok(repositorioVeiculos.ObterVeiculosAgrupadosPorGrupo());
    }

    public void AlugarVeiculo(int id)
    {
        var veiculoSelecionado = repositorioVeiculos.SelecionarPorId(id);
        veiculoSelecionado!.Alugado = true;

        repositorioVeiculos.Editar(veiculoSelecionado);
    }

    public void LiberarVeiculo(int id)
    {
        var veiculoSelecionado = repositorioVeiculos.SelecionarPorId(id);
        veiculoSelecionado!.Alugado = false;

        repositorioVeiculos.Editar(veiculoSelecionado);
    }

    public bool VeiculoRelacionadoAoGrupo(GrupoDeAutomoveis registro) 
        => repositorioVeiculos.SelecionarTodos().Any(c => c.GrupoDeAutomoveis.Id == registro.Id);

    public bool ValidarRegistroRepetido(Veiculo novoRegistro)
    {
        var registrosExistentes = repositorioVeiculos.SelecionarTodos();
        var registroAtual = novoRegistro.Id == 0 ? new() : repositorioVeiculos.SelecionarPorId(novoRegistro.Id);

        return registrosExistentes.Exists(r => r.Placa == novoRegistro.Placa && r.Placa != registroAtual!.Placa);
    }

    public bool SemRegistros()
        => repositorioVeiculos.SelecionarTodos().Count == 0;
}