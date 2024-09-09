using LocadoraDeVeiculos.Dominio.ModuloTaxa;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloTaxa;

public class MapeadorTaxaEmOrm : IEntityTypeConfiguration<Taxa>
{
    public void Configure(EntityTypeBuilder<Taxa> tBuilder)
    {
        tBuilder.ToTable("TBTaxa");

        tBuilder.Property(t => t.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        tBuilder.Property(t => t.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        tBuilder.Property(t => t.Preco)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        tBuilder.Property(t => t.PrecoFixo)
            .IsRequired()
            .HasColumnType("bit");

        tBuilder.Property(t => t.Seguro)
            .IsRequired()
            .HasColumnType("bit");

        tBuilder.Property(t => t.Ativo)
            .HasColumnType("bit");

        tBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        tBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}