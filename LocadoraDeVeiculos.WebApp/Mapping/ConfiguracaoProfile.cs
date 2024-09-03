using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;
public class ConfiguracaoProfile : Profile
{
    public ConfiguracaoProfile()
    {
        CreateMap<InserirConfiguracaoViewModel, Configuracao>();
        CreateMap<EditarConfiguracaoViewModel, Configuracao>();
        CreateMap<Configuracao, DetalhesConfiguracaoViewModel>();
        CreateMap<Configuracao, EditarConfiguracaoViewModel>();
    }
}