using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;

public class VeiculoProfile : Profile
{
    public VeiculoProfile()
    {
        CreateMap<InserirVeiculosViewModel, Veiculo>();
        CreateMap<EditarVeiculosViewModel, Veiculo>();
        CreateMap<Veiculo, ListarVeiculosViewModel>();
        CreateMap<Veiculo, DetalhesVeiculosViewModel>();
        CreateMap<Veiculo, EditarVeiculosViewModel>();
    }
}