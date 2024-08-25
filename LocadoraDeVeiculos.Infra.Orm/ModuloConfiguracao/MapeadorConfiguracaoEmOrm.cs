using LocadoraDeVeiculos.Dominio.ModuloConfiguracao;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloConfiguracao;
public class MapeadorConfiguracaoEmOrm : IEntityTypeConfiguration<Configuracao>
{
    public void Configure(EntityTypeBuilder<Configuracao> cBuilder)
    {
        cBuilder.ToTable("TBConfiguracao");

        cBuilder.Property(c => c.Gasolina)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        cBuilder.Property(c => c.Diesel)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        cBuilder.Property(c => c.Etanol)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        cBuilder.Property(c => c.GNV)
            .IsRequired()
            .HasColumnType("decimal(18,2)");


        /*        cBuilder.Property(s => s.UsuarioId)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("Usuario_Id");

                cBuilder.HasOne(g => g.Usuario)
                    .WithMany()
                    .HasForeignKey(s => s.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction);*/
    }
}