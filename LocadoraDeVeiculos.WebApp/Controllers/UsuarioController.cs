using LocadoraDeVeiculos.Dominio.ModuloUsuario;
using LocadoraDeVeiculos.WebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace LocadoraDeVeiculos.WebApp.Controllers;
public class UsuarioController(UserManager<Usuario> userManager, SignInManager<Usuario> signInManager, RoleManager<Perfil> roleManager) : Controller
{
    public IActionResult Registrar() => View(new RegistrarViewModel());


    [HttpPost]
    public async Task<IActionResult> Registrar(RegistrarViewModel registrarVm)
    {
        if (!ModelState.IsValid)
            return View(registrarVm);

        var usuario = new Usuario()
        {
            UserName = registrarVm.Usuario,
            Email = registrarVm.Email
        };

        var resultadoCriacaoUsuario = await userManager.CreateAsync(usuario, registrarVm.Senha!);

        var resultadoCriacaoTipoUsuario = await roleManager.FindByNameAsync("Empresa");

        if (resultadoCriacaoTipoUsuario is null)
        {
            var cargo = new Perfil()
            {
                Name = "Empresa",
                NormalizedName ="EMPRESA",
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await roleManager.CreateAsync(cargo);
        }

        await userManager.AddToRoleAsync(usuario, "Empresa");

        if (resultadoCriacaoUsuario.Succeeded)
        {
            await signInManager.SignInAsync(usuario, isPersistent: false);
            return RedirectToAction("Index", "Home");
        }

        foreach (var erro in resultadoCriacaoUsuario.Errors)
            ModelState.AddModelError(string.Empty, erro.Description);

        return View(registrarVm);
    }


    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;

        if (!ModelState.IsValid)
            return View(model);

        var resultadoLogin = await signInManager.PasswordSignInAsync(
            model.Usuario!,
            model.Senha!,
            false,
            false
        );

        if (resultadoLogin.Succeeded)
            return LocalRedirect(returnUrl ?? "/");

        ModelState.AddModelError(string.Empty, "Login ou senha inválidos");

        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    public IActionResult AcessoNegado() => View();
}
