using LocadoraDeVeiculos.Dominio.ModuloAluguel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloAluguel;
public class MapeadorAluguelEmOrm : IEntityTypeConfiguration<Aluguel>
{
    public void Configure(EntityTypeBuilder<Aluguel> aBuilder)
    {
        aBuilder.ToTable("TBAluguel");

        aBuilder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        aBuilder.Property(a => a.CondutorNome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        aBuilder.Property(a => a.CondutorId)
            .IsRequired()
            .HasColumnType("int");

        aBuilder.Property(a => a.ClienteNome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        aBuilder.Property(a => a.ClienteId)
            .IsRequired()
            .HasColumnType("int");

        aBuilder.Property(a => a.CategoriaPlano)
            .HasConversion<string>()
            .IsRequired();

        aBuilder.Property(a => a.GrupoNome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        aBuilder.Property(a => a.GrupoId)
            .IsRequired()
            .HasColumnType("int");

        aBuilder.Property(a => a.VeiculoId)
            .IsRequired()
            .HasColumnType("int");

        aBuilder.Property(a => a.VeiculoPlaca)
            .IsRequired()
            .HasColumnType("varchar(10)");

        aBuilder.Property(c => c.DataSaida)
            .IsRequired()
            .HasColumnType("datetime2");

        aBuilder.Property(c => c.DataRetornoPrevista)
            .IsRequired()
            .HasColumnType("datetime2");

        aBuilder.Property(c => c.DataRetornoReal)
            .HasColumnType("datetime2");

        aBuilder.Property(c => c.Ativo)
            .IsRequired()
            .HasColumnType("bit");

        aBuilder.Property(a => a.ValorTotal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        aBuilder.Property(a => a.TaxasSelecionadasId)
            .HasColumnType("varchar(10)");

        aBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        aBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}