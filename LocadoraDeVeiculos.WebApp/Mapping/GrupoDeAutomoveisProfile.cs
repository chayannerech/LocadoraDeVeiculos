using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;

public class GrupoDeAutomoveisProfile : Profile
{
    public GrupoDeAutomoveisProfile()
    {
        CreateMap<InserirGrupoDeAutomoveisViewModel, GrupoDeAutomoveis>();
        CreateMap<EditarGrupoDeAutomoveisViewModel, GrupoDeAutomoveis>();
        CreateMap<GrupoDeAutomoveis, ListarGrupoDeAutomoveisViewModel>();
        CreateMap<GrupoDeAutomoveis, DetalhesGrupoDeAutomoveisViewModel>();
        CreateMap<GrupoDeAutomoveis, EditarGrupoDeAutomoveisViewModel>();
    }
}