using FluentResults;
using LocadoraDeVeiculos.Aplicacao.Servicos;
using LocadoraDeVeiculos.Dominio.Compartilhado;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.WebApp.Extensions;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace LocadoraDeVeiculos.WebApp.Controllers.Compartilhado;
public abstract class WebControllerBase : Controller
{
    protected int? UsuarioId
    {
        get
        {
            var usuarioAutenticado = User.FindFirst(ClaimTypes.NameIdentifier);

            if (usuarioAutenticado is null)
                return null;

            return int.Parse(usuarioAutenticado.Value);
        }
    }

    protected IActionResult MensagemRegistroNaoEncontrado(int idRegistro)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Erro",
            Mensagem = $"Não foi possível encontrar o registro ID [{idRegistro}]!"
        });

        return RedirectToAction("Index", "Inicio");
    }

    protected void ApresentarMensagemFalha(Result resultado)
    {
        ViewBag.Mensagem = new MensagemViewModel
        {
            Titulo = "Falha",
            Mensagem = resultado.Errors[0].Message
        };
    }

    protected void ApresentarMensagemRegistroExistente(string mensagemDeErro)
    {
        ViewBag.Mensagem = new MensagemViewModel
        {
            Titulo = "Falha",
            Mensagem = mensagemDeErro
        };
    }

    protected void ApresentarMensagemSemRegistros()
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Vazio",
            Mensagem = $"Ainda não existem itens cadastrados"
        });
    }

    protected void ApresentarMensagemSemDependencias(string dependencia)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Aviso",
            Mensagem = $"Não é possível inserir um novo registro. Não existem {dependencia} cadastrados"
        });
    }

    protected void ApresentarMensagemImpossivelEditar(string dependencia)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Aviso",
            Mensagem = $"Não é possível editar este registro. {dependencia}"
        });
    }

    protected void ApresentarMensagemImpossivelExcluir(string dependencia)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Aviso",
            Mensagem = $"Não é possível excluir este registro. {dependencia}"
        });
    }

    protected void ApresentarMensagemSucesso(string mensagem)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = mensagem
        });
    }
    protected void ApresentarMensagemFalha(string mensagem)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Falha",
            Mensagem = mensagem
        });
    }

}