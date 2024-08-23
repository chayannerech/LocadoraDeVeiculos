using LocadoraDeVeiculos.Dominio.ModuloPlanoDeCobranca;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloPlanoDeCobranca;

public class MapeadorPlanoDeCobrancaEmOrm : IEntityTypeConfiguration<PlanoDeCobranca>
{
    public void Configure(EntityTypeBuilder<PlanoDeCobranca> sBuilder)
    {
        sBuilder.ToTable("TBPlanoDeCobranca");

        sBuilder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        sBuilder.Property(s => s.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        sBuilder.HasMany(s => s.Planos)
            .WithOne(p => p.PlanoDeCobranca)
            .HasForeignKey("Grupo_Id");


        /*        sBuilder.Property(s => s.UsuarioId)
                    .IsRequired()
                    .HasColumnType("int")
                    .HasColumnName("Usuario_Id");

                sBuilder.HasOne(g => g.Usuario)
                    .WithMany()
                    .HasForeignKey(s => s.UsuarioId)
                    .OnDelete(DeleteBehavior.NoAction);*/
    }
}