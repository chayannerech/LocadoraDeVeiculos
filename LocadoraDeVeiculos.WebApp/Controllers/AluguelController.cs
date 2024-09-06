using AutoMapper;
using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
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

        if (servicoAluguel.SemRegistros(UsuarioId.GetValueOrDefault()) && ViewBag.Mensagem is null)
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

        Funcionario funcionario = servicoFuncionario.AtribuirFuncionarioAoAluguel(User, UsuarioId.GetValueOrDefault());

        var resultado = servicoAluguel.Inserir(novoRegistro, inserirRegistroVm.CondutorId, inserirRegistroVm.ClienteId, inserirRegistroVm.GrupoId, inserirRegistroVm.VeiculoId, funcionario);

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

        servicoVeiculo.LiberarVeiculo(editarRegistroVm.VeiculoId);

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

        servicoVeiculo.LiberarVeiculo(servicoAluguel.SelecionarPorId(detalhesRegistroVm.Id).Value.Veiculo.Id);

        var resultado = servicoAluguel.Desativar(detalhesRegistroVm.Id);

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
        devolverVm.DataRetornoReal = DateTime.MinValue;

        return View(CarregarInformacoes(devolverVm));
    }
    [HttpPost]
    public IActionResult Devolver(DevolverAluguelViewModel devolverRegistroVm)
    {
        if (!ModelState.IsValid)
            return View(CarregarInformacoes(devolverRegistroVm));

        var resultado = servicoAluguel.Devolver(devolverRegistroVm.Id, devolverRegistroVm.ValorTotal, devolverRegistroVm.DataRetornoReal);

        servicoVeiculo.LiberarVeiculo(servicoAluguel.SelecionarPorId(devolverRegistroVm.Id).Value.Veiculo.Id);
        servicoVeiculo.AtualizarKmRodados(servicoAluguel.SelecionarPorId(devolverRegistroVm.Id).Value.Veiculo.Id, devolverRegistroVm.KmFinal);

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
        var seguros = resultadoTaxas.Value.FindAll(t => t.Seguro);

        inserirRegistroVm.Condutores = condutores;
        inserirRegistroVm.Clientes = clientes.Select(c => new SelectListItem(c.Nome, c.Id.ToString()));
        inserirRegistroVm.Grupos = grupos;
        inserirRegistroVm.Veiculos = veiculos;
        inserirRegistroVm.Categorias = Enum.GetNames(typeof(CategoriaDePlanoEnum)).Select(c => new SelectListItem(c, c));
        inserirRegistroVm.Taxas = taxas;

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

        return editarRegistroVm;
    }
    private DevolverAluguelViewModel? CarregarInformacoes(DevolverAluguelViewModel devolverRegistroVm)
    {
        List<Taxa> taxas = [];

        var registro = servicoAluguel.SelecionarPorId(devolverRegistroVm.Id).Value;

        var clienteResultado = servicoCliente.SelecionarPorId(registro.Cliente.Id);
        var condutorResultado = servicoCondutor.SelecionarPorId(registro.Condutor.Id);
        var grupoResultado = servicoGrupo.SelecionarPorId(registro.Grupo.Id);
        var veiculoResultado = servicoVeiculo.SelecionarPorId(registro.Veiculo.Id);
        var planoResultado = servicoPlano.SelecionarPorGrupoId(registro.Grupo.Id);
        var configResultado = servicoConfiguracao.Selecionar(UsuarioId.GetValueOrDefault());


        if (clienteResultado.IsFailed || condutorResultado.IsFailed || grupoResultado.IsFailed || veiculoResultado.IsFailed)
        {
            ApresentarMensagemFalha(Result.Fail("Falha ao encontrar dados necessários!"));
            return null;
        }

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
        devolverRegistroVm.DataSaida = registro.DataSaida;
        devolverRegistroVm.DataRetornoPrevista = registro.DataRetornoPrevista;

        if (devolverRegistroVm.TaxasSelecionadasId != "")
            foreach (var taxaId in devolverRegistroVm.TaxasSelecionadasId!.Split(','))
                taxas.Add(servicoTaxa.SelecionarPorId(Convert.ToInt32(taxaId)).Value);

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
        if (!registro.AluguelAtivo)
        {
            ApresentarMensagemImpossivelEditar("Aluguel já finalizado");
            return true;
        }
        return false;
    }
    private bool AluguelAtivo(Aluguel registro)
    {
        if (registro.AluguelAtivo)
        {
            ApresentarMensagemImpossivelExcluir("Aluguel ativo");
            return true;
        }
        return false;
    }
    private bool SemDependencias()
    {
        if (servicoCliente.SemRegistros(UsuarioId) || servicoCondutor.SemRegistros(UsuarioId.GetValueOrDefault()) || servicoGrupo.SemRegistros(UsuarioId) || servicoVeiculo.SemRegistros(UsuarioId.GetValueOrDefault()) || servicoPlano.SemRegistros(UsuarioId.GetValueOrDefault()) || servicoConfiguracao.SemRegistros(UsuarioId.GetValueOrDefault()))
        {
            if (servicoCliente.SemRegistros(UsuarioId))
                ApresentarMensagemSemDependencias("Clientes");

            if (servicoCondutor.SemRegistros(UsuarioId.GetValueOrDefault()))
                ApresentarMensagemSemDependencias("Condutores");

            if (servicoGrupo.SemRegistros(UsuarioId.GetValueOrDefault()))
                ApresentarMensagemSemDependencias("Grupos de Automóveis");

            if (servicoVeiculo.SemRegistros(UsuarioId.GetValueOrDefault()))
                ApresentarMensagemSemDependencias("Veículos");

            if (servicoPlano.SemRegistros(UsuarioId.GetValueOrDefault()))
                ApresentarMensagemSemDependencias("Planos de Aluguel");

            if (servicoConfiguracao.SemRegistros(UsuarioId.GetValueOrDefault()))
                ApresentarMensagemSemDependencias("Preços dos Combustíveis");

            return true;
        }
        return false;
    }
    #endregion
}