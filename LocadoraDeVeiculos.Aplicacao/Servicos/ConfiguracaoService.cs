using FluentResults;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracaoe;
namespace LocadoraDeVeiculos.Aplicacao.Servicos;
public class ConfiguracaoService(IRepositorioConfiguracao repositorioConfiguracao)
{
    public Result<Configuracao> Inserir(Configuracao registro)
    {
        repositorioConfiguracao.Inserir(registro); 
        return Result.Ok(registro);
    }
    public Result<Configuracao> Editar(Configuracao registroAtualizado)
    {
        var registro = repositorioConfiguracao.Selecionar(registroAtualizado.UsuarioId);

        if (registro is null)
            return Result.Fail("Ainda não existe uma configuração cadastrada!");

        registro.Gasolina = registroAtualizado.Gasolina;
        registro.Diesel = registroAtualizado.Diesel;
        registro.GNV = registroAtualizado.GNV;
        registro.Etanol = registroAtualizado.Etanol;

        repositorioConfiguracao.Editar(registro);

        return Result.Ok(registro);
    }
    public Configuracao Selecionar(int usuarioId)
        => repositorioConfiguracao.Selecionar(usuarioId);
    public bool SemRegistros(int usuarioId)
        => Selecionar(usuarioId) is null;
}