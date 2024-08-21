using Microsoft.AspNetCore.Identity;
namespace LocadoraDeVeiculos.Dominio.ModuloUsuario;

public class Usuario : IdentityUser<int>
{
    public Usuario() => EmailConfirmed = true;
}