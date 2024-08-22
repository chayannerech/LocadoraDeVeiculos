using LocadoraDeVeiculos.Dominio.ModuloVeiculos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloVeiculos;

public class MapeadorVeiculosEmOrm : IEntityTypeConfiguration<Veiculos>
{
    public void Configure(EntityTypeBuilder<Veiculos> vBuilder)
    {
        vBuilder.ToTable("TBVeiculos");

        vBuilder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        vBuilder.Property(s => s.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

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