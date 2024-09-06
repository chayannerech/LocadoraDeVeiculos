using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;

namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class VeiculoService(IRepositorioVeiculo repositorioVeiculo, IRepositorioGrupoDeAutomoveis repositorioGrupo)
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

        repositorioVeiculo.Inserir(registro);

        return Result.Ok(registro);
    }

    public Result<Veiculo> Editar(Veiculo registroAtualizado, int grupoId)
    {
        var registro = repositorioVeiculo.SelecionarPorId(registroAtualizado.Id);

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

        repositorioVeiculo.Editar(registro);

        return Result.Ok(registro);
    }

    public Result Excluir(int registroId)
    {
        var registro = repositorioVeiculo.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        repositorioVeiculo.Excluir(registro);

        return Result.Ok();
    }

    public Result<Veiculo> SelecionarPorId(int registroId)
    {
        var registro = repositorioVeiculo.SelecionarPorId(registroId);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        return Result.Ok(registro);
    }

    public Result<List<Veiculo>> SelecionarTodos(int usuarioId)
        => Result.Ok(repositorioVeiculo.Filtrar(f => f.UsuarioId == usuarioId));

    public Result<List<IGrouping<string, Veiculo>>> ObterVeiculosAgrupadosPorGrupo(int? usuarioId)
    {
        if (usuarioId > 0)
            return Result.Ok(repositorioVeiculo.ObterVeiculosAgrupadosPorGrupo(usuarioId.Value));

        return Result.Ok(repositorioVeiculo.ObterVeiculosAgrupadosPorGrupo());
    }

    public void AlugarVeiculo(int id)
    {
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(id);
        veiculoSelecionado!.Alugado = true;

        repositorioVeiculo.Editar(veiculoSelecionado);
    }

    public void LiberarVeiculo(int id)
    {
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(id);
        veiculoSelecionado!.Alugado = false;

        repositorioVeiculo.Editar(veiculoSelecionado);
    }

    public bool VeiculoRelacionadoAoGrupo(GrupoDeAutomoveis registro) 
        => repositorioVeiculo.SelecionarTodos().Any(v => v.GrupoDeAutomoveis.Id == registro.Id);

    public bool ValidarRegistroRepetido(Veiculo novoRegistro, int? usuarioId)
    {
        var registrosExistentes = repositorioVeiculo.Filtrar(v => v.UsuarioId == usuarioId);
        var registroAtual = novoRegistro.Id == 0 ? new() : repositorioVeiculo.SelecionarPorId(novoRegistro.Id);

        return registrosExistentes.Exists(r => r.Placa == novoRegistro.Placa && r.Placa != registroAtual!.Placa);
    }

    public bool SemRegistros(int? usuarioId)
        => !repositorioVeiculo.SelecionarTodos().Any(v => v.UsuarioId == usuarioId);

    public bool SemRegistros()
        => repositorioVeiculo.SelecionarTodos().Count == 0;

    public Result<Veiculo> Desativar(int id)
    {
        var registro = repositorioVeiculo.SelecionarPorId(id);

        if (registro is null)
            return Result.Fail("O veículo não foi encontrado!");

        registro.Ativo = false;

        repositorioVeiculo.Editar(registro);

        return Result.Ok();
    }

    public void AtualizarKmRodados(int id, int kmAtualizado)
    {
        var veiculoSelecionado = repositorioVeiculo.SelecionarPorId(id);
        veiculoSelecionado!.KmRodados = kmAtualizado;

        repositorioVeiculo.Editar(veiculoSelecionado);
    }
}