using DevIO.NerdStore.Pagamentos.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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