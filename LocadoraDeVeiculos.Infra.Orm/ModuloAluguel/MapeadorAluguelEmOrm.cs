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

        #region Dependências
        aBuilder.HasOne(a => a.Condutor)
            .WithMany()
            .HasForeignKey("Condutor_Id")
            .IsRequired();

        aBuilder.HasOne(a => a.Cliente)
            .WithMany()
            .HasForeignKey("Cliente_Id")
            .IsRequired();

        aBuilder.HasOne(a => a.Grupo)
            .WithMany()
            .HasForeignKey("Grupo_Id")
            .IsRequired();

        aBuilder.HasOne(a => a.Plano)
            .WithMany()
            .HasForeignKey("Plano_Id")
            .IsRequired();

        aBuilder.HasOne(a => a.Veiculo)
            .WithMany()
            .HasForeignKey("Veiculo_Id")
            .IsRequired();

        aBuilder.HasOne(a => a.Configuracao)
            .WithMany()
            .HasForeignKey("Configuracao_Id")
            .IsRequired();

        aBuilder.HasOne(a => a.Funcionario)
            .WithMany()
            .HasForeignKey("Funcionario_Id")
            .IsRequired();

        aBuilder.Ignore(a => a.Taxas);
        #endregion

        #region Retirada
        aBuilder.Property(a => a.Entrada)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        aBuilder.Property(a => a.ValorTotal)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        aBuilder.Property(a => a.DataSaida)            
            .HasColumnType("datetime2")
            .IsRequired();

        aBuilder.Property(a => a.DataRetornoPrevista)
            .HasColumnType("datetime")
            .IsRequired();

        aBuilder.Property(a => a.DataRetornoReal)
            .HasColumnType("datetime")
            .IsRequired();

        aBuilder.Property(a => a.TaxasSelecionadasId)
            .HasColumnType("nvarchar(max)")
            .IsRequired();

        aBuilder.Property(a => a.Ativo)
            .HasColumnType("bit");
        #endregion

        #region Devolução
        aBuilder.Property(a => a.KmInicial).
            HasColumnType("int")
            .IsRequired();

        aBuilder.Property(a => a.KmFinal)
            .HasColumnType("int")
            .IsRequired();

        aBuilder.Property(a => a.TanqueCheio)
            .HasColumnType("bit")
            .IsRequired();
        #endregion
    }
}