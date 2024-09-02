using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDePlanoDeCobranca.WebApp.Controllers;
public class PlanoDeCobrancaController(PlanoDeCobrancaService servicoPlano, GrupoDeAutomoveisService servicoGrupo, AluguelService servicoAluguel, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoPlano.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var registros = resultado.Value;

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        if (servicoPlano.SemRegistros() && ViewBag.Mensagem is null)
            ApresentarMensagemSemRegistros();

        var listarRegistrosVm = mapeador.Map<IEnumerable<ListarPlanoDeCobrancaViewModel>>(registros);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(listarRegistrosVm);
    }

    public IActionResult Inserir()
    {
        if (servicoGrupo.SemRegistros())
        {
            ApresentarMensagemSemDependencias("Grupos de Automóveis");
            return RedirectToAction(nameof(Listar));
        }
        return View(CarregarInformacoes(new InserirPlanoDeCobrancaViewModel()));
    }
    [HttpPost]
    public IActionResult Inserir(InserirPlanoDeCobrancaViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<PlanoDeCobranca>(inserirRegistroVm);

        if (servicoPlano.ValidarRegistroRepetido(novoRegistro, inserirRegistroVm.GrupoId))
        {
            ApresentarMensagemRegistroExistente("Este grupo de automóveis já possui um plano de cobrança");
            return View(CarregarInformacoes(inserirRegistroVm));
        }

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoPlano.Inserir(novoRegistro, inserirRegistroVm.GrupoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoGrupo.AdicionarValores(novoRegistro);

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoPlano.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var editarRegistroVm = mapeador.Map<EditarPlanoDeCobrancaViewModel>(registro);

        editarRegistroVm.GrupoId = registro.GrupoDeAutomoveis.Id;

        return View(CarregarInformacoes(editarRegistroVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarPlanoDeCobrancaViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(editarRegistroVm));

        var registro = mapeador.Map<PlanoDeCobranca>(editarRegistroVm);

        if (servicoPlano.ValidarRegistroRepetido(registro, editarRegistroVm.GrupoId))
        {
            ApresentarMensagemRegistroExistente("Este grupo de automóveis já possui um plano de cobrança");
            return View(CarregarInformacoes(editarRegistroVm));
        }

        var resultado = servicoPlano.Editar(registro, editarRegistroVm.GrupoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoGrupo.AdicionarValores(servicoPlano.SelecionarPorId(registro.Id).Value);

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoPlano.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var detalhesRegistroVm = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesPlanoDeCobrancaViewModel detalhesRegistroVm)
    {
        var nome = servicoGrupo.SelecionarPorId(detalhesRegistroVm.Id).Value.Nome;
        servicoGrupo.ExcluirValores(servicoPlano.SelecionarPorId(detalhesRegistroVm.Id).Value);

        var resultado = servicoPlano.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"Plano para o grupo: {nome}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoPlano.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesPlanoDeCobrancaViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region Auxiliares
    private InserirPlanoDeCobrancaViewModel? CarregarInformacoes(InserirPlanoDeCobrancaViewModel inserirRegistroVm)
    {
        var resultadoGrupos = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        var grupos = resultadoGrupos.Value;

        inserirRegistroVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        inserirRegistroVm.Categorias = new(Enum.GetNames(typeof(CategoriaDePlanoEnum)));

        return inserirRegistroVm;
    }
    private EditarPlanoDeCobrancaViewModel? CarregarInformacoes(EditarPlanoDeCobrancaViewModel editarRegistroVm)
    {
        var resultadoGrupos = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

        var grupos = resultadoGrupos.Value;

        editarRegistroVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        editarRegistroVm.Categorias = new(Enum.GetNames(typeof(CategoriaDePlanoEnum)));

        return editarRegistroVm;
    }
    protected bool ValidarFalha(Result<PlanoDeCobranca> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidarFalhaLista(Result<List<PlanoDeCobranca>> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    #endregion
}