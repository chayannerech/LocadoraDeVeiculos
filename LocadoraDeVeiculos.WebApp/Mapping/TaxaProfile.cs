using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;
public class TaxaProfile : Profile
{
    public TaxaProfile()
    {
        CreateMap<InserirTaxaViewModel, Taxa>();
        CreateMap<EditarTaxaViewModel, Taxa>();
        CreateMap<Taxa, ListarTaxaViewModel>();
        CreateMap<Taxa, DetalhesTaxaViewModel>();
        CreateMap<Taxa, EditarTaxaViewModel>();
    }
}