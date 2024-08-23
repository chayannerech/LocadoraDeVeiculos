using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;

public class PlanoDeCobrancaProfile : Profile
{
    public PlanoDeCobrancaProfile()
    {
        CreateMap<InserirPlanoDeCobrancaViewModel, PlanoDeCobranca>();
        CreateMap<EditarPlanoDeCobrancaViewModel, PlanoDeCobranca>();
        CreateMap<PlanoDeCobranca, ListarPlanoDeCobrancaViewModel>();
        CreateMap<PlanoDeCobranca, DetalhesPlanoDeCobrancaViewModel>();
        CreateMap<PlanoDeCobranca, EditarPlanoDeCobrancaViewModel>();
    }
}