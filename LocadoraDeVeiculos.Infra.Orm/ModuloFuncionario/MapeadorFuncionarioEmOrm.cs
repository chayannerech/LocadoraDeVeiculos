using LocadoraDeVeiculos.Dominio.ModuloFuncionario;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloFuncionario;
public class MapeadorFuncionarioEmOrm : IEntityTypeConfiguration<Funcionario>
{
    public void Configure(EntityTypeBuilder<Funcionario> fBuilder)
    {
        fBuilder.ToTable("TBFuncionario");

        fBuilder.Property(f => f.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        fBuilder.Property(f => f.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        fBuilder.Property(f => f.Login)
            .IsRequired()
            .HasColumnType("varchar(200)");

        fBuilder.Property(f => f.DataAdmissao)
            .IsRequired()
            .HasColumnType("datetime2");

        fBuilder.Property(a => a.Salario)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        fBuilder.Property(f => f.Ativo)
            .HasColumnType("bit");

        fBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        fBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}