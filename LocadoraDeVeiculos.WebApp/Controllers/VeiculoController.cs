using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class VeiculoController(VeiculoService servicoVeiculos, GrupoDeAutomoveisService servicoGrupos, IMapper mapeador) : WebControllerBase
{
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

        if (agrupamentos.Count == 0)
            ApresentarMensagemSemRegistros();

        var agrupamentosVeiculosVm = agrupamentos.Select(MapearAgrupamentoVeiculos);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(agrupamentosVeiculosVm);
    }


    public IActionResult Inserir() => View(CarregarInformacoes(new InserirVeiculosViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirVeiculosViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<Veiculo>(inserirRegistroVm);

        if (ValidacaoDeRegistroRepetido(servicoVeiculos, novoRegistro, null))
            return View(CarregarInformacoes(inserirRegistroVm));

        //novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoVeiculos.Inserir(novoRegistro, inserirRegistroVm.GrupoId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Editar(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var veiculo = resultado.Value;

        var editarRegistroVm = mapeador.Map<EditarVeiculosViewModel>(veiculo);

        editarRegistroVm.GrupoId = veiculo.GrupoDeAutomoveis.Id;

        return View(CarregarInformacoes(editarRegistroVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarVeiculosViewModel editarRegistroVm)
    {
        if (!ModelState.IsValid)
        {
            if (editarRegistroVm.Foto != null)
            {
                editarRegistroVm.ImagemEmBytes = ConverterImagemParaArrayDeBytes(editarRegistroVm.Foto);
                editarRegistroVm.TipoDaImagem = editarRegistroVm.Foto.ContentType;
            }
            else return View(CarregarInformacoes(editarRegistroVm)); 
        }

        var registro = mapeador.Map<Veiculo>(editarRegistroVm);

        var registroAtual = servicoVeiculos.SelecionarPorId(editarRegistroVm.Id).Value;

        if (ValidacaoDeRegistroRepetido(servicoVeiculos, registro, registroAtual))
            return View(CarregarInformacoes(editarRegistroVm));

        var resultado = servicoVeiculos.Editar(registro, editarRegistroVm.GrupoId);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Excluir(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesVeiculosViewModel = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        return View(detalhesVeiculosViewModel);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesVeiculosViewModel detalhesVeiculosViewModel)
    {
        var resultado = servicoVeiculos.Excluir(detalhesVeiculosViewModel.Id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{detalhesVeiculosViewModel.Placa}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoVeiculos.SelecionarPorId(id);

        if (ValidacaoDeFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesVeiculosViewModel = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        detalhesVeiculosViewModel.GrupoNome = registro.GrupoDeAutomoveis.Nome;

        return View(detalhesVeiculosViewModel);
    }

    #region Auxiliares
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

        inserirVeiculosVm.TiposDeCombustiveis = Enum.GetNames(typeof(TipoDeCombustivelEnum)).Select(t =>
            new SelectListItem(t, t));

        return inserirVeiculosVm;
    }
    private EditarVeiculosViewModel? CarregarInformacoes(EditarVeiculosViewModel editarVeiculosVm)
    {
        var resultadoGrupos = servicoGrupos.SelecionarTodos(UsuarioId.GetValueOrDefault());

        if (resultadoGrupos.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));

            return null;
        }

        var grupos = resultadoGrupos.Value;

        editarVeiculosVm.Grupos = grupos.Select(g =>
            new SelectListItem(g.Nome, g.Id.ToString()));

        editarVeiculosVm.TiposDeCombustiveis = Enum.GetNames(typeof(TipoDeCombustivelEnum)).Select(t =>
            new SelectListItem(t, t));

        return editarVeiculosVm;
    }
    private AgrupamentoVeiculosPorGrupoViewModel MapearAgrupamentoVeiculos(IGrouping<string, Veiculo> grp)
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
    protected bool ValidacaoDeFalha(Result<Veiculo> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private bool ValidacaoDeRegistroRepetido(VeiculoService servicoVeiculo, Veiculo novoRegistro, Veiculo registroAtual)
    {
        var registrosExistentes = servicoVeiculo.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        registroAtual = registroAtual is null ? new() : registroAtual;

        if (registrosExistentes.Exists(r =>
            r.Placa == novoRegistro.Placa &&
            r.Placa != registroAtual.Placa))
        {
            ApresentarMensagemRegistroExistente();
            return true;
        }
        return false;
    }
    #endregion
}