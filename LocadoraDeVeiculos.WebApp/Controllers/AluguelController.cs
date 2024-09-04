using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
namespace LocadoraDeAluguel.WebApp.Controllers;

[Authorize(Roles = "Empresa, Funcionario")]
public class AluguelController(
        AluguelService servicoAluguel,
        CondutorService servicoCondutor,
        ClienteService servicoCliente,
        PlanoDeCobrancaService servicoPlano,
        GrupoDeAutomoveisService servicoGrupo,
        VeiculoService servicoVeiculo,
        TaxaService servicoTaxa,
        ConfiguracaoService servicoConfiguracao,
        FuncionarioService servicoFuncionario,
        IMapper mapeador) : WebControllerBase(servicoFuncionario)
{
    public IActionResult Listar()
    {
        var resultado =
            servicoAluguel.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return RedirectToAction("Index", "Inicio");
        }

        var registros = resultado.Value;

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        if (registros.Count == 0 && ViewBag.Mensagem is null)
            ApresentarMensagemSemRegistros();

        var listarAluguelVm = mapeador.Map<IEnumerable<ListarAluguelViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarAluguelVm);
    }


    public IActionResult Inserir()
    {
        if (SemDependencias())
            return RedirectToAction(nameof(Listar));

        return View(CarregarInformacoes(new InserirAluguelViewModel()));
    }
    [HttpPost]
    public IActionResult Inserir(InserirAluguelViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
        {
            if (inserirRegistroVm.CategoriaPlano != CategoriaDePlanoEnum.Diário)
            return View(CarregarInformacoes(inserirRegistroVm));
        }

        inserirRegistroVm.TaxasSelecionadasId ??= "";

        var novoRegistro = mapeador.Map<Aluguel>(inserirRegistroVm);

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoAluguel.Inserir(novoRegistro, inserirRegistroVm.CondutorId, inserirRegistroVm.ClienteId, inserirRegistroVm.GrupoId, inserirRegistroVm.VeiculoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoVeiculo.AlugarVeiculo(inserirRegistroVm.VeiculoId);

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (AluguelFinalizado(registro))
            return RedirectToAction(nameof(Listar));

        var editarRegistroVm = mapeador.Map<EditarAluguelViewModel>(registro);

        return View(CarregarInformacoes(editarRegistroVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarAluguelViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
        {
            if (editarRegistroVm.CategoriaPlano != CategoriaDePlanoEnum.Diário)
            return View(CarregarInformacoes(editarRegistroVm));
        }

        servicoVeiculo.LiberarVeiculo(servicoAluguel.SelecionarPorId(editarRegistroVm.Id).Value.VeiculoId);

        var registro = mapeador.Map<Aluguel>(editarRegistroVm);

        var resultado = servicoAluguel.Editar(registro, editarRegistroVm.CondutorId, editarRegistroVm.ClienteId, editarRegistroVm.GrupoId, editarRegistroVm.VeiculoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoVeiculo.AlugarVeiculo(editarRegistroVm.VeiculoId);

        var nome = servicoAluguel.SelecionarPorId(editarRegistroVm.Id).Value;

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (AluguelAtivo(registro))
            return RedirectToAction(nameof(Listar));

        var detalhesRegistroVm = mapeador.Map<DetalhesAluguelViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesAluguelViewModel detalhesRegistroVm)
    {
        var nome = servicoAluguel.SelecionarPorId(detalhesRegistroVm.Id).Value;

        servicoVeiculo.LiberarVeiculo(detalhesRegistroVm.VeiculoId);

        var resultado = servicoAluguel.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Devolver(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var devolverVm = mapeador.Map<DevolverAluguelViewModel>(registro);

        return View(CarregarInformacoes(devolverVm));
    }
    [HttpPost]
    public IActionResult Devolver(DevolverAluguelViewModel devolverRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(devolverRegistroVm));

        var resultado = servicoAluguel.Devolver(devolverRegistroVm.Id, devolverRegistroVm.ValorTotal, devolverRegistroVm.DataRetornoReal);

        servicoVeiculo.LiberarVeiculo(servicoAluguel.SelecionarPorId(devolverRegistroVm.Id).Value.VeiculoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var nome = servicoAluguel.SelecionarPorId(devolverRegistroVm.Id).Value;

        ApresentarMensagemSucesso($"O registro \"{nome}\" foi devolvido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesAluguelViewModel>(registro);

        return View(CarregarInformacoes(detalhesRegistroVm));
    }

    #region Auxiliares
    private InserirAluguelViewModel? CarregarInformacoes(InserirAluguelViewModel inserirRegistroVm)
    {
        var resultadoCondutores = servicoCondutor.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoClientes = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoGrupos = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoVeiculos = servicoVeiculo.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoTaxas = servicoTaxa.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoCondutores.IsFailed || resultadoClientes.IsFailed || resultadoGrupos.IsFailed || resultadoVeiculos.IsFailed || resultadoTaxas.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        var condutores = resultadoCondutores.Value;
        var clientes = resultadoClientes.Value;
        var grupos = resultadoGrupos.Value;
        var veiculos = resultadoVeiculos.Value;
        var taxas = resultadoTaxas.Value;
        var seguros = resultadoTaxas.Value.FindAll(t => t.Seguro);

        inserirRegistroVm.Condutores = condutores;
        inserirRegistroVm.Clientes = clientes.Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
        inserirRegistroVm.Grupos = grupos;
        inserirRegistroVm.Veiculos = veiculos;
        inserirRegistroVm.Categorias = Enum.GetNames(typeof(CategoriaDePlanoEnum)).Select(c => new SelectListItem(c, c));
        inserirRegistroVm.Taxas = taxas;
        inserirRegistroVm.Seguros = seguros;

        return inserirRegistroVm;
    }
    private EditarAluguelViewModel? CarregarInformacoes(EditarAluguelViewModel editarRegistroVm)
    {
        var resultadoCondutores = servicoCondutor.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoClientes = servicoCliente.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoGrupos = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoVeiculos = servicoVeiculo.SelecionarTodos(UsuarioId.GetValueOrDefault());
        var resultadoTaxas = servicoTaxa.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoCondutores.IsFailed || resultadoClientes.IsFailed || resultadoGrupos.IsFailed || resultadoVeiculos.IsFailed || resultadoTaxas.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        var condutores = resultadoCondutores.Value;
        var clientes = resultadoClientes.Value;
        var grupos = resultadoGrupos.Value;
        var veiculos = resultadoVeiculos.Value;
        var taxas = resultadoTaxas.Value;
        var seguros = resultadoTaxas.Value.FindAll(t => t.Seguro);

        editarRegistroVm.Condutores = condutores;
        editarRegistroVm.Clientes = clientes.Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
        editarRegistroVm.Grupos = grupos;
        editarRegistroVm.Veiculos = veiculos;
        editarRegistroVm.Categorias = Enum.GetNames(typeof(CategoriaDePlanoEnum)).Select(c => new SelectListItem(c, c));
        editarRegistroVm.Taxas = taxas;
        editarRegistroVm.Seguros = seguros;

        return editarRegistroVm;
    }
    private DetalhesAluguelViewModel? CarregarInformacoes(DetalhesAluguelViewModel detalhesRegistroVm)
    {
        List<Taxa> taxas = [];

        var veiculoSelecionado = servicoVeiculo.SelecionarPorId(detalhesRegistroVm.VeiculoId);

        if (veiculoSelecionado.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        detalhesRegistroVm.ImagemEmBytes = veiculoSelecionado.Value.ImagemEmBytes;
        detalhesRegistroVm.TipoDaImagem = veiculoSelecionado.Value.TipoDaImagem;

        return detalhesRegistroVm;
    }
    private DevolverAluguelViewModel? CarregarInformacoes(DevolverAluguelViewModel devolverRegistroVm)
    {
        List<Taxa> taxas = [];

        var registro = servicoAluguel.SelecionarPorId(devolverRegistroVm.Id).Value;

        var clienteResultado = servicoCliente.SelecionarPorId(registro.ClienteId);
        var condutorResultado = servicoCondutor.SelecionarPorId(registro.CondutorId);
        var grupoResultado = servicoGrupo.SelecionarPorId(registro.GrupoId);
        var veiculoResultado = servicoVeiculo.SelecionarPorId(registro.VeiculoId);
        var planoResultado = servicoPlano.SelecionarPorGrupoId(registro.GrupoId);
        var configResultado = servicoConfiguracao.Selecionar();


        if (clienteResultado.IsFailed || condutorResultado.IsFailed || grupoResultado.IsFailed || veiculoResultado.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        if (devolverRegistroVm.TaxasSelecionadasId != "")
            foreach (var taxaId in devolverRegistroVm.TaxasSelecionadasId!.Split(','))
                taxas.Add(servicoTaxa.SelecionarPorId(Convert.ToInt32(taxaId)).Value);

        devolverRegistroVm.Cliente = clienteResultado.Value;
        devolverRegistroVm.Condutor = condutorResultado.Value;
        devolverRegistroVm.Grupo = grupoResultado.Value;
        devolverRegistroVm.Veiculo = veiculoResultado.Value;
        devolverRegistroVm.Configuracao = configResultado;
        devolverRegistroVm.TaxasSelecionadasId = registro.TaxasSelecionadasId;
        devolverRegistroVm.Taxas = taxas;
        devolverRegistroVm.ImagemEmBytes = veiculoResultado.Value.ImagemEmBytes;
        devolverRegistroVm.TipoDaImagem = veiculoResultado.Value.TipoDaImagem;
        devolverRegistroVm.PlanoDeCobranca = planoResultado.Value;

        return devolverRegistroVm;
    }
    protected bool ValidarFalha(Result<Aluguel> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool AluguelFinalizado(Aluguel registro)
    {
        if (!registro.Ativo)
        {
            ApresentarMensagemImpossivelEditar("Aluguel já finalizado");
            return true;
        }
        return false;
    }
    private bool AluguelAtivo(Aluguel registro)
    {
        if (registro.Ativo)
        {
            ApresentarMensagemImpossivelExcluir("Aluguel ativo");
            return true;
        }
        return false;
    }
    private bool SemDependencias()
    {
        if (servicoCliente.SemRegistros(UsuarioId) || servicoCondutor.SemRegistros() || servicoGrupo.SemRegistros() || servicoVeiculo.SemRegistros() || servicoPlano.SemRegistros() || servicoConfiguracao.SemRegistros())
        {
            if (servicoCliente.SemRegistros(UsuarioId))
                ApresentarMensagemSemDependencias("Clientes");

            if (servicoCondutor.SemRegistros())
                ApresentarMensagemSemDependencias("Condutores");

            if (servicoGrupo.SemRegistros())
                ApresentarMensagemSemDependencias("Grupos de Automóveis");

            if (servicoVeiculo.SemRegistros())
                ApresentarMensagemSemDependencias("Veículos");

            if (servicoPlano.SemRegistros())
                ApresentarMensagemSemDependencias("Planos de Aluguel");

            if (servicoConfiguracao.SemRegistros())
                ApresentarMensagemSemDependencias("Preços dos Combustíveis");

            return true;
        }
        return false;
    }
    #endregion
}