using LocadoraDeVeiculos.Dominio.ModuloCondutor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloCondutor;
public class MapeadorCondutorEmOrm : IEntityTypeConfiguration<Condutor>
{
    public void Configure(EntityTypeBuilder<Condutor> cBuilder)
    {
        cBuilder.ToTable("TBCondutor");

        cBuilder.Property(c => c.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        cBuilder.Property(c => c.Nome)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Email)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Telefone)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.HasOne(v => v.Cliente)
            .WithMany()
            .HasForeignKey("Cliente_Id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        cBuilder.Property(c => c.CPF)
            .IsRequired()
            .HasColumnType("varchar(15)");

        cBuilder.Property(c => c.CNH)
            .IsRequired()
            .HasColumnType("varchar(15)");

        cBuilder.Property(c => c.ValidadeCNH)
            .IsRequired()
            .HasColumnType("datetime2");

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