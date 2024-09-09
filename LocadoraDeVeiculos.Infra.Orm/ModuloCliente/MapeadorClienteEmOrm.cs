using LocadoraDeVeiculos.Dominio.ModuloCliente;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace LocadoraDeVeiculos.Infra.Orm.ModuloCliente;
public class MapeadorClienteEmOrm : IEntityTypeConfiguration<Cliente>
{
    public void Configure(EntityTypeBuilder<Cliente> cBuilder)
    {
        cBuilder.ToTable("TBCliente");

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

        cBuilder.Property(c => c.Documento)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.RG)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.CNH)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Estado)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Cidade)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Bairro)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Rua)
            .IsRequired()
            .HasColumnType("varchar(200)");

        cBuilder.Property(c => c.Numero)
            .IsRequired()
            .HasColumnType("int");

        cBuilder.Property(c => c.PessoaFisica)
            .IsRequired()
            .HasColumnType("bit");

        cBuilder.Property(c => c.Ativo)
            .HasColumnType("bit");

        cBuilder.Property(s => s.UsuarioId)
            .IsRequired()
            .HasColumnType("int")
            .HasColumnName("Usuario_Id");

        cBuilder.HasOne(g => g.Usuario)
            .WithMany()
            .HasForeignKey(s => s.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}