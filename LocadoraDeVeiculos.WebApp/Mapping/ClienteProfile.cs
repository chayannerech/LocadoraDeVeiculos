using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloCliente;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;
public class ClienteProfile : Profile
{
    public ClienteProfile()
    {
        CreateMap<InserirClienteViewModel, Cliente>();
        CreateMap<EditarClienteViewModel, Cliente>();
        CreateMap<Cliente, ListarClienteViewModel>();
        CreateMap<Cliente, DetalhesClienteViewModel>();
        CreateMap<Cliente, EditarClienteViewModel>();
    }
}