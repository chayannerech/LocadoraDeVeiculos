using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;
public class AluguelProfile : Profile
{
    public AluguelProfile()
    {
        CreateMap<InserirAluguelViewModel, Aluguel>();
        CreateMap<EditarAluguelViewModel, Aluguel>();
        CreateMap<Aluguel, ListarAluguelViewModel>();
        CreateMap<Aluguel, DetalhesAluguelViewModel>();
        CreateMap<Aluguel, EditarAluguelViewModel>();
    }
}