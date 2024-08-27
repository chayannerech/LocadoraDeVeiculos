using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeAluguel.WebApp.Controllers;
public class AluguelController(
        AluguelService servicoAluguel,
        CondutorService servicoCondutor,
        ClienteService servicoCliente,
        PlanoDeCobrancaService servicoPlano,
        GrupoDeAutomoveisService servicoGrupo,
        VeiculoService servicoVeiculo,
        TaxaService servicoTaxa,
        IMapper mapeador) : WebControllerBase
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

        if (registros.Count == 0)
            ApresentarMensagemSemRegistros();

        var listarAluguelVm = mapeador.Map<IEnumerable<ListarAluguelViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarAluguelVm);
    }


    public IActionResult Inserir() => View(CarregarInformacoes(new InserirAluguelViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirAluguelViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<Aluguel>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoAluguel, inserirRegistroVm, null))
            return View(inserirRegistroVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoAluguel.Inserir(novoRegistro, inserirRegistroVm.CondutorId, inserirRegistroVm.ClienteId, inserirRegistroVm.GrupoId, inserirRegistroVm.VeiculoId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var editarPlanoVm = mapeador.Map<EditarAluguelViewModel>(registro);

        editarPlanoVm.GrupoId = registro.GrupoDeAutomoveis.Id;

        return View(CarregarInformacoes(editarPlanoVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarAluguelViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(editarRegistroVm);

        var registro = mapeador.Map<Aluguel>(editarRegistroVm);
        var registroAtual = servicoAluguel.SelecionarPorId(editarRegistroVm.Id).Value;

        if (ValidacaoDeRegistroRepetido(servicoAluguel, editarRegistroVm, registroAtual))
            return View(editarRegistroVm);

        var resultado = servicoAluguel.Editar(registro, editarRegistroVm.CondutorId, editarRegistroVm.ClienteId, editarRegistroVm.GrupoId, editarRegistroVm.VeiculoId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesAluguelViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesAluguelViewModel detalhesRegistroVm)
    {
        var resultado = servicoAluguel.Excluir(detalhesRegistroVm.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"Plano para o grupo: {detalhesRegistroVm.GrupoNome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Devolver(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesAluguelViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Devolver(DetalhesAluguelViewModel detalhesRegistroVm)
    {
        var resultado = servicoAluguel.Excluir(detalhesRegistroVm.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"Plano para o grupo: {detalhesRegistroVm.GrupoNome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoAluguel.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesAluguelViewModel>(registro);

        return View(detalhesRegistroVm);
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
        var seguros = taxas.FindAll(t => t.Seguro);

        inserirRegistroVm.Condutores = condutores;
        inserirRegistroVm.Clientes = clientes.Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
        inserirRegistroVm.Grupos = grupos.Select(g => new SelectListItem(g.Nome, g.Id.ToString()));
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
        var seguros = taxas.FindAll(t => t.Seguro);

        editarRegistroVm.Condutores = condutores;
        editarRegistroVm.Clientes = clientes.Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
        editarRegistroVm.Grupos = grupos.Select(g => new SelectListItem(g.Nome, g.Id.ToString()));
        editarRegistroVm.Veiculos = veiculos;
        editarRegistroVm.Categorias = Enum.GetNames(typeof(CategoriaDePlanoEnum)).Select(c => new SelectListItem(c, c));
        editarRegistroVm.Taxas = taxas;
        editarRegistroVm.Seguros = seguros;
        return editarRegistroVm;
    }
    protected bool ValidacaoDeFalha(Result<Aluguel> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(AluguelService servicoAluguel, InserirAluguelViewModel novoRegistro, Aluguel registroAtual)
    {
        var registrosExistentes = servicoAluguel.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() : registroAtual;

        if (registrosExistentes.Exists(r =>
            r.Veiculo.Id == novoRegistro.VeiculoId &&
            r.GrupoDeAutomoveis.Id != registroAtual.GrupoDeAutomoveis.Id))
        {
            ApresentarMensagemRegistroExistente();
            return true;
        }
        return false;
    }
    #endregion
}