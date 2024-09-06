using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;
public class MapeadorGrupoDeAutomoveisEmOrm : IEntityTypeConfiguration<GrupoDeAutomoveis>
{
    public void Configure(EntityTypeBuilder<GrupoDeAutomoveis> gBuilder)
    {
        gBuilder.ToTable("TBGrupoDeAutomoveis");

        gBuilder.Property(g => g.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        gBuilder.Property(g => g.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        gBuilder.Property(p => p.PrecoDiaria)
            .HasColumnType("decimal(18,2)");

        gBuilder.Property(p => p.PrecoDiariaControlada)
            .HasColumnType("decimal(18,2)");

        gBuilder.Property(p => p.PrecoLivre)
            .HasColumnType("decimal(18,2)");

        gBuilder.Property(p => p.Ativo)
            .HasColumnType("bit");

        gBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        gBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}