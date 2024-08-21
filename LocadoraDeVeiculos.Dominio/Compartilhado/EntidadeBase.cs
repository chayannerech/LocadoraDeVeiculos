﻿using LocadoraDeVeiculos.Dominio.ModuloUsuario;
namespace LocadoraDeVeiculos.Dominio.Compartilhado;
public abstract class EntidadeBase
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }
}