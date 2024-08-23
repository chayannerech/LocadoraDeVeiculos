using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class VeiculosController(VeiculosService servicoVeiculos, GrupoDeAutomoveisService servicoGrupos, IMapper mapeador) : WebControllerBase
{
    private readonly IMapper mapeador = mapeador;
    public IActionResult Listar()
    {
        var resultado =
            servicoVeiculos.ObterVeiculosAgrupadosPorGrupo(UsuarioId.GetValueOrDefault());

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var agrupamentos = resultado.Value;

        var agrupamentosVeiculosVm = agrupamentos
            .Select(MapearAgrupamentoVeiculos);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(agrupamentosVeiculosVm);
    }

    public IActionResult Inserir() => View(CarregarInformacoes(new InserirVeiculosViewModel()));

    [HttpPost]
    public IActionResult Inserir(InserirVeiculosViewModel inserirVeiculosVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirVeiculosVm));

        var novoRegistro = mapeador.Map<Veiculos>(inserirVeiculosVm);

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoVeiculos.Inserir(novoRegistro, inserirVeiculosVm.GrupoId);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Editar(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(resultadoGrupos.ToResult());

            return null;
        }

        var veiculo = resultado.Value;

        var editarVm = mapeador.Map<EditarVeiculosViewModel>(veiculo);

        var gruposDisponiveis = resultadoGrupos.Value;

        editarVm.GrupoId = veiculo.GrupoDeAutomoveis.Id;

        editarVm.Grupos = gruposDisponiveis
            .Select(g => new SelectListItem(g.Nome, g.Id.ToString()));

        return View(editarVm);
    }

    [HttpPost]
    public IActionResult Editar(EditarVeiculosViewModel editarVeiculosVm)
    {
        if (!ModelState.IsValid)
            return View(editarVeiculosVm);

        if (editarVeiculosVm.Foto != null)
        {
            editarVeiculosVm.ImagemEmBytes = ConverterImagemParaArrayDeBytes(editarVeiculosVm.Foto);
            editarVeiculosVm.TipoDaImagem = editarVeiculosVm.Foto.ContentType;
        }

        var registro = mapeador.Map<Veiculos>(editarVeiculosVm);

        var resultado = servicoVeiculos.Editar(registro, editarVeiculosVm.GrupoId);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Excluir(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesVeiculosViewModel = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        return View(detalhesVeiculosViewModel);
    }

    [HttpPost]
    public IActionResult Excluir(DetalhesVeiculosViewModel detalhesVeiculosViewModel)
    {
        var resultado = servicoVeiculos.Excluir(detalhesVeiculosViewModel.Id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado);

            return RedirectToAction(nameof(Listar));
        }

        ApresentarMensagemSucesso($"O registro \"{detalhesVeiculosViewModel.Placa}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }

    public IActionResult Detalhes(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());

            return RedirectToAction(nameof(Listar));
        }

        var registro = resultado.Value;

        var detalhesVeiculosViewModel = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        return View(detalhesVeiculosViewModel);
    }

    private InserirVeiculosViewModel? CarregarInformacoes(InserirVeiculosViewModel inserirVeiculosVm)
    {
        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));

            return null;
        }

        var grupos = resultadoGrupos.Value;

        inserirVeiculosVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        return inserirVeiculosVm;
    }
    private AgrupamentoVeiculosPorGrupoViewModel MapearAgrupamentoVeiculos(IGrouping<string, Veiculos> grp)
        => new()
        {
            Grupo = grp.Key,
            Veiculos = mapeador.Map<IEnumerable<ListarVeiculosViewModel>>(grp)
        };
    private byte[] ConverterImagemParaArrayDeBytes(IFormFile imagem)
    {
        if (imagem == null || imagem.Length == 0)
        {
            throw new ArgumentNullException(nameof(imagem), "A imagem não pode ser nula ou vazia");
        }

        using var memoryStream = new MemoryStream();
        imagem.CopyTo(memoryStream);
        return memoryStream.ToArray();
    }
}