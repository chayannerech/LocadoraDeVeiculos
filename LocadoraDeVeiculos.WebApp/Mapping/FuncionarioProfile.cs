using AutoMapper;
using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using LocadoraDeVeiculos.WebApp.Models;
namespace LocadoraDeVeiculos.WebApp.Mapping;
public class FuncionarioProfile : Profile
{
    public FuncionarioProfile()
    {
        CreateMap<InserirFuncionarioViewModel, Funcionario>();
        CreateMap<EditarFuncionarioViewModel, Funcionario>();
        CreateMap<Funcionario, ListarFuncionarioViewModel>();
        CreateMap<Funcionario, DetalhesFuncionarioViewModel>();
        CreateMap<Funcionario, EditarFuncionarioViewModel>();
    }
}