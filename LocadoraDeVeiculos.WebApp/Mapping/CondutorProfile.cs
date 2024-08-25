using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;
public class CondutorProfile : Profile
{
    public CondutorProfile()
    {
        CreateMap<InserirCondutorViewModel, Condutor>();
        CreateMap<EditarCondutorViewModel, Condutor>();
        CreateMap<Condutor, ListarCondutorViewModel>();
        CreateMap<Condutor, DetalhesCondutorViewModel>();
        CreateMap<Condutor, EditarCondutorViewModel>();
    }
}