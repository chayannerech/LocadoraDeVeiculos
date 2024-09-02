using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobranca;
public class MapeadorPlanoDeCobrancaEmOrm : IEntityTypeConfiguration<PlanoDeCobranca>
{
    public void Configure(EntityTypeBuilder<PlanoDeCobranca> pBuilder)
    {
        pBuilder.ToTable("TBPlanoDeCobranca");

        pBuilder.Property(p => p.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        pBuilder.HasOne(p => p.GrupoDeAutomoveis)
            .WithMany()
            .HasForeignKey("Grupo_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        pBuilder.Property(p => p.PrecoDiaria)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        pBuilder.Property(p => p.PrecoKm)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        pBuilder.Property(p => p.KmDisponivel)
            .IsRequired()
            .HasColumnType("int");

        pBuilder.Property(p => p.PrecoDiariaControlada)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        pBuilder.Property(p => p.PrecoExtrapolado)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        pBuilder.Property(p => p.PrecoLivre)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        pBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        pBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}