using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;

public class VeiculosProfile : Profile
{
    public VeiculosProfile()
    {
        CreateMap<InserirVeiculosViewModel, Veiculos>();
        CreateMap<EditarVeiculosViewModel, Veiculos>();
        CreateMap<Veiculos, ListarVeiculosViewModel>();
        CreateMap<Veiculos, DetalhesVeiculosViewModel>();
        CreateMap<Veiculos, EditarVeiculosViewModel>();
    }
}