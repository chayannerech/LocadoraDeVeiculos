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

        aBuilder.HasOne(v => v.Condutor)
            .WithMany()
            .HasForeignKey("Condutor_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        aBuilder.HasOne(v => v.Cliente)
            .WithMany()
            .HasForeignKey("Cliente_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        aBuilder.HasOne(v => v.PlanoDeCobranca)
            .WithMany()
            .HasForeignKey("Plano_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        aBuilder.HasOne(v => v.GrupoDeAutomoveis)
            .WithMany()
            .HasForeignKey("Grupo_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        aBuilder.HasOne(v => v.Veiculo)
            .WithMany()
            .HasForeignKey("Veiculo_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        aBuilder.Property(c => c.DataSaida)
            .IsRequired()
            .HasColumnType("datetime2");

        aBuilder.Property(c => c.DataRetorno)
            .IsRequired()
            .HasColumnType("datetime2");

        aBuilder.Property(c => c.SeguroCondutor)
            .IsRequired()
            .HasColumnType("bit");

        aBuilder.Property(c => c.SeguroTerceiro)
            .IsRequired()
            .HasColumnType("bit");

        /*        aBuilder.Property(s => s.UsuarioId)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("Usuario_Id");

                aBuilder.HasOne(g => g.Usuario)
                    .WithMany()
                    .HasForeignKey(s => s.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction);*/
    }
}