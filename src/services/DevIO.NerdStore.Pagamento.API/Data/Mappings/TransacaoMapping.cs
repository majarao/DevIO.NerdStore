using DevIO.NerdStore.Pagamentos.API.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace DevIO.NerdStore.Pagamentos.API.Data.Mappings;

public class TransacaoMapping : IEntityTypeConfiguration<Transacao>
{
    public void Configure(EntityTypeBuilder<Transacao> builder)
    {
        builder.HasKey(c => c.Id);

        builder
            .HasOne(c => c.Pagamento)
            .WithMany(c => c.Transacoes);

        builder.ToTable("Transacoes");
    }
}