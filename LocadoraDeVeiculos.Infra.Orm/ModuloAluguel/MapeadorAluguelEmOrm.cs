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
            .OnDelete(DeleteBehavior.NoAction);

        aBuilder.Property(a => a.CondutorNome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        aBuilder.HasOne(v => v.Cliente)
            .WithMany()
            .HasForeignKey("Cliente_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        aBuilder.Property(a => a.ClienteNome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        aBuilder.HasOne(v => v.PlanoDeCobranca)
            .WithMany()
            .HasForeignKey("Plano_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        aBuilder.Property(a => a.CategoriaPlano)
            .HasConversion<string>()
            .IsRequired();

        aBuilder.HasOne(v => v.GrupoDeAutomoveis)
            .WithMany()
            .HasForeignKey("Grupo_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

        aBuilder.Property(a => a.GrupoNome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        aBuilder.HasOne(v => v.Veiculo)
            .WithMany()
            .HasForeignKey("Veiculo_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.NoAction);

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

        aBuilder.Property(c => c.SeguroCondutor)
            .IsRequired()
            .HasColumnType("bit");

        aBuilder.Property(c => c.SeguroTerceiro)
            .IsRequired()
            .HasColumnType("bit");

        aBuilder.Property(a => a.ValorTotal)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

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