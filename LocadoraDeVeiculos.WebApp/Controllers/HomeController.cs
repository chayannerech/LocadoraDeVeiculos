using AutoMapper;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;

public class HomeController(
    GrupoDeAutomoveisService servicoGrupo, 
    VeiculoService servicoVeiculo, 
    ClienteService servicoCliente,
    CondutorService servicoCondutor,
    PlanoDeCobrancaService servicoPlano,
    TaxaService servicoTaxa,
    AluguelService servicoAluguel,
    FuncionarioService servicoFuncionario,
    IMapper mapeador) : WebControllerBase(servicoFuncionario)
{

    public ViewResult Index()
    {
        var resultadoAgrupamentos =
            servicoVeiculo.ObterVeiculosAgrupadosPorGrupo(UsuarioId.GetValueOrDefault());

        var agrupamentos = resultadoAgrupamentos.Value;

        var agrupamentosSessoesVm = agrupamentos.Select(MapearAgrupamentoVeiculos);

        ViewBag.Agrupamentos = agrupamentosSessoesVm;

        if (UsuarioId.HasValue)
        {
            ViewBag.QuantidadeClientes = servicoCliente.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeCondutores = servicoCondutor.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeGrupos = servicoGrupo.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeVeiculos = servicoVeiculo.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadePlanos = servicoPlano.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeTaxas = servicoTaxa.SelecionarTodos(UsuarioId.Value).Value.Count;
            ViewBag.QuantidadeAlugueis = servicoAluguel.SelecionarTodos(UsuarioId.Value).Value.Count;
        }

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View();
    }

    private AgrupamentoVeiculosPorGrupoViewModel MapearAgrupamentoVeiculos(IGrouping<string, Veiculo> agrupamento)
    {
        return new AgrupamentoVeiculosPorGrupoViewModel
        {
            Grupo = agrupamento.Key,
            Veiculos = mapeador.Map<IEnumerable<ListarVeiculosViewModel>>(agrupamento)
        };
    }
}