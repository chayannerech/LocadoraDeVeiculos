using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;
public class MapeadorVeiculoEmOrm : IEntityTypeConfiguration<Veiculo>
{
    public void Configure(EntityTypeBuilder<Veiculo> vBuilder)
    {
        vBuilder.ToTable("TBVeiculos");

        vBuilder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        vBuilder.Property(v => v.Placa)
            .IsRequired()
            .HasColumnType("varchar(10)");

        vBuilder.Property(v => v.Marca)
            .IsRequired()
            .HasColumnType("varchar(50)");

        vBuilder.Property(v => v.Cor)
            .IsRequired()
            .HasColumnType("varchar(30)");

        vBuilder.Property(v => v.Modelo)
            .IsRequired()
            .HasColumnType("varchar(50)");

        vBuilder.Property(v => v.TipoCombustivel)
            .IsRequired()
            .HasColumnType("varchar(50)");

        vBuilder.Property(v => v.CapacidadeCombustivel)
            .IsRequired()
            .HasColumnType("int");

        vBuilder.Property(v => v.Ano)
            .IsRequired()
            .HasColumnType("int");

        vBuilder.Property(v => v.ImagemEmBytes)
            .IsRequired()
            .HasColumnType("varbinary(max)");

        vBuilder.Property(v => v.TipoDaImagem)
            .IsRequired()
            .HasColumnType("nvarchar(255)");

        vBuilder.HasOne(v => v.GrupoDeAutomoveis)
            .WithMany()
            .HasForeignKey("Grupo_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        vBuilder.Property(v => v.Alugado)
            .IsRequired()
            .HasColumnType("bit");

        /*        vBuilder.Property(s => s.UsuarioId)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("Usuario_Id");

                vBuilder.HasOne(g => g.Usuario)
                    .WithMany()
                    .HasForeignKey(s => s.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction);*/
    }
}