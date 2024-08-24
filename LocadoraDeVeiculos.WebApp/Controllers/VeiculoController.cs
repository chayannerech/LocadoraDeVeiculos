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
public class VeiculoController(VeiculoService servicoVeiculos, GrupoDeAutomoveisService servicoGrupos, IMapper mapeador) : WebControllerBase
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

        if (agrupamentos.Count == 0)
            ApresentarMensagemSemRegistros();

        var agrupamentosVeiculosVm = agrupamentos.Select(MapearAgrupamentoVeiculos);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(agrupamentosVeiculosVm);
    }


    public IActionResult Inserir() => View(CarregarInformacoes(new InserirVeiculosViewModel()));
    [HttpPost]
    public IActionResult Inserir(InserirVeiculosViewModel inserirVeiculosVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirVeiculosVm));

        var novoRegistro = mapeador.Map<Veiculo>(inserirVeiculosVm);

        var registrosExistentes = servicoVeiculos.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.Placa == novoRegistro.Placa))
        {
            ApresentarMensagemRegistroExistente();
            return View(CarregarInformacoes(inserirVeiculosVm));
        }

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

        var veiculo = resultado.Value;

        var editarVeiculoVm = mapeador.Map<EditarVeiculosViewModel>(veiculo);

        editarVeiculoVm.GrupoId = veiculo.GrupoDeAutomoveis.Id;

        return View(CarregarInformacoes(editarVeiculoVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarVeiculosViewModel editarVeiculosVm)
    {
        if (!ModelState.IsValid)
        {
            if (editarVeiculosVm.Foto != null)
            {
                editarVeiculosVm.ImagemEmBytes = ConverterImagemParaArrayDeBytes(editarVeiculosVm.Foto);
                editarVeiculosVm.TipoDaImagem = editarVeiculosVm.Foto.ContentType;
            }
            else return View(editarVeiculosVm); 
        }

        var registro = mapeador.Map<Veiculo>(editarVeiculosVm);

        var registroAtual = servicoVeiculos.SelecionarPorId(editarVeiculosVm.Id).Value;
        var registrosExistentes = servicoVeiculos.SelecionarTodos(UsuarioId.GetValueOrDefault()).Value;

        if (registrosExistentes.Exists(r => r.Placa == registro.Placa && r.Placa != registroAtual.Placa))
        {
            ApresentarMensagemRegistroExistente();
            return View(CarregarInformacoes(editarVeiculosVm));
        }

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
    #endregion
}