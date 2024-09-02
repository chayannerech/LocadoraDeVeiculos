using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.Dominio.ModuloVeiculo;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Win32;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class VeiculoController(VeiculoService servicoVeiculo, GrupoDeAutomoveisService servicoGrupo, AluguelService servicoAluguel, IMapper mapeador) : WebControllerBase
{
    public IActionResult Listar()
    {
        var resultado = servicoVeiculo.ObterVeiculosAgrupadosPorGrupo(UsuarioId.GetValueOrDefault());

        if (!User.Identity!.IsAuthenticated)
            resultado = servicoVeiculo.ObterVeiculosAgrupadosPorGrupo();

        if (ValidarFalhaLista(resultado))
            return RedirectToAction(nameof(Listar));

        var agrupamentos = resultado.Value;

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        if (agrupamentos.Count == 0 && ViewBag.Mensagem is null)
            ApresentarMensagemSemRegistros();

        var agrupamentosVeiculosVm = agrupamentos.Select(MapearAgrupamentoVeiculos);

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();

        return View(agrupamentosVeiculosVm);
    }


    [Authorize(Roles = "Empresa, Funcionário")]
    public IActionResult Inserir()
    {
        if (servicoGrupo.SemRegistros())
        {
            ApresentarMensagemSemDependencias("Grupos de Automóveis");
            return RedirectToAction(nameof(Listar));
        }

        return View(CarregarInformacoes(new InserirVeiculosViewModel()));
    }
    [HttpPost]
    public IActionResult Inserir(InserirVeiculosViewModel inserirRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(inserirRegistroVm));

        var novoRegistro = mapeador.Map<Veiculo>(inserirRegistroVm);

        if (servicoVeiculo.ValidarRegistroRepetido(novoRegistro))
        {
            ApresentarMensagemRegistroExistente("Já existe um veículo com essa placa");
            return View(CarregarInformacoes(inserirRegistroVm));
        }

        novoRegistro.UsuarioId = UsuarioId.GetValueOrDefault();

        var resultado = servicoVeiculo.Inserir(novoRegistro, inserirRegistroVm.GrupoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O registro \"{novoRegistro}\" foi inserido com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Roles = "Empresa, Funcionário")]
    public IActionResult Editar(int id)
    {
        var resultado = servicoVeiculo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelEditar("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var editarRegistroVm = mapeador.Map<EditarVeiculosViewModel>(registro);

        editarRegistroVm.GrupoId = registro.GrupoDeAutomoveis.Id;

        return View(CarregarInformacoes(editarRegistroVm));
    }
    [HttpPost]
    public IActionResult Editar(EditarVeiculosViewModel editarRegistroVm)
    {
        AjustarDadosDaFoto(ref editarRegistroVm, servicoVeiculo.SelecionarPorId(editarRegistroVm.Id).Value);

        if (!ModelState.IsValid)
            View(CarregarInformacoes(editarRegistroVm));

        var registro = mapeador.Map<Veiculo>(editarRegistroVm);

        if (servicoVeiculo.ValidarRegistroRepetido(registro))
        {
            ApresentarMensagemRegistroExistente("Já existe um veículo com essa placa");
            return View(CarregarInformacoes(editarRegistroVm));
        }

        var resultado = servicoVeiculo.Editar(registro, editarRegistroVm.GrupoId);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        servicoAluguel.AtualizarVeiculoDoAluguel(registro);

        ApresentarMensagemSucesso($"O registro \"{registro}\" foi editado com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    [Authorize(Roles = "Empresa, Funcionário")]
    public IActionResult Excluir(int id)
    {
        var resultado = servicoVeiculo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        if (servicoAluguel.AluguelRelacionadoAtivo(registro))
        {
            ApresentarMensagemImpossivelExcluir("Existe um aluguel ativo relacionado");
            return RedirectToAction(nameof(Listar));
        }

        var detalhesRegistroVm = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        return View(detalhesRegistroVm);
    }
    [HttpPost]
    public IActionResult Excluir(DetalhesVeiculosViewModel detalhesRegistroVm)
    {
        var resultado = servicoVeiculo.Excluir(detalhesRegistroVm.Id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        ApresentarMensagemSucesso($"O veículo de placa \"{servicoVeiculo.SelecionarPorId(detalhesRegistroVm.Id).Value.Placa}\" foi excluído com sucesso!");

        return RedirectToAction(nameof(Listar));
    }


    public IActionResult Detalhes(int id)
    {
        var resultado = servicoVeiculo.SelecionarPorId(id);

        if (ValidarFalha(resultado))
            return RedirectToAction(nameof(Listar));

        var registro = resultado.Value;

        var detalhesRegistroVm = mapeador.Map<DetalhesVeiculosViewModel>(registro);

        return View(detalhesRegistroVm);
    }

    #region Auxiliares
    private InserirVeiculosViewModel? CarregarInformacoes(InserirVeiculosViewModel inserirVeiculosVm)
    {
        var resultadoGrupos = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

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
        var resultadoGrupos = servicoGrupo.SelecionarTodos(UsuarioId.GetValueOrDefault());

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
    protected bool ValidarFalha(Result<Veiculo> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    protected bool ValidarFalhaLista(Result<List<IGrouping<string, Veiculo>>> resultado)
    {
        if (resultado.IsFailed)
        {
            ApresentarMensagemFalha(resultado.ToResult());
            return true;
        }
        return false;
    }
    private void AjustarDadosDaFoto(ref EditarVeiculosViewModel editarRegistroVm, Veiculo registroAtual)
    {
        if (editarRegistroVm.Foto != null)
        {
            editarRegistroVm.ImagemEmBytes = ConverterImagemParaArrayDeBytes(editarRegistroVm.Foto);
            editarRegistroVm.TipoDaImagem = editarRegistroVm.Foto.ContentType;
        }
        else
        {
            editarRegistroVm.ImagemEmBytes = registroAtual.ImagemEmBytes;
            editarRegistroVm.TipoDaImagem = registroAtual.TipoDaImagem;
        }

        ModelState.Remove(nameof(editarRegistroVm.ImagemEmBytes));
        ModelState.Remove(nameof(editarRegistroVm.TipoDaImagem));
        ModelState.Remove(nameof(editarRegistroVm.Foto));
    }
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