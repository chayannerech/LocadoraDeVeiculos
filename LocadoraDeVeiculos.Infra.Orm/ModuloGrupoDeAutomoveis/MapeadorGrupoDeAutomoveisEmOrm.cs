using LocadoraDeVeiculos.Dominio.ModuloGrupoDeAutomoveis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloGrupoDeAutomoveis;

public class MapeadorGrupoDeAutomoveisEmOrm : IEntityTypeConfiguration<GrupoDeAutomoveis>
{
    public void Configure(EntityTypeBuilder<GrupoDeAutomoveis> sBuilder)
    {
        sBuilder.ToTable("TBGrupoDeAutomoveis");

        sBuilder.Property(s => s.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        sBuilder.Property(s => s.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        sBuilder.HasMany(s => s.Planos)
            .WithOne(p => p.GrupoDeAutomoveis)
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