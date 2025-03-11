using DevIO.NerdStore.Pagamentos.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pagamentos.API.Data.Mappings;

public class PagamentoMapping : IEntityTypeConfiguration<Pagamento>
{
    public void Configure(EntityTypeBuilder<Pagamento> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Ignore(c => c.CartaoCredito);

        builder
            .HasMany(c => c.Transacoes)
            .WithOne(c => c.Pagamento)
            .HasForeignKey(c => c.PagamentoId);

        builder.ToTable("Pagamentos");
    }
}