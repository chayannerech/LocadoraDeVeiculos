using FluentResults;
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

        ViewBag.Mensagem = TempData.DesserializarMensagemViewModel();
    }

    protected void ApresentarMensagemSemRegistros()
    {
        ViewBag.Mensagem = new MensagemViewModel
        {
            Titulo = "Vazio",
            Mensagem = $"Ainda não existem itens cadastrados"
        };
    }
    protected void ApresentarMensagemImpossivelExcluir()
    {
        ViewBag.Mensagem = new MensagemViewModel
        {
            Titulo = "Aviso",
            Mensagem = $"Não é possível excluir este registro"
        };
    }

    protected void ApresentarMensagemSucesso(string mensagem)
    {
        TempData.SerializarMensagemViewModel(new MensagemViewModel
        {
            Titulo = "Sucesso",
            Mensagem = mensagem
        });
    }
}