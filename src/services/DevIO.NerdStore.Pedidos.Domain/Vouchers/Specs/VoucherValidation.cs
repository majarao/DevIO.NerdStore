using DevIO.NerdStore.Core.Specification.Validation;

namespace DevIO.NerdStore.Pedidos.Domain.Vouchers.Specs;

public class VoucherValidation : SpecValidator<Voucher>
{
    public VoucherValidation()
    {
        VoucherDataSpecification dataSpec = new();
        VoucherQuantidadeSpecification qtdeSpec = new();
        VoucherAtivoSpecification ativoSpec = new();

        Add("dataSpec", new Rule<Voucher>(dataSpec, "Este voucher está expirado"));
        Add("qtdeSpec", new Rule<Voucher>(qtdeSpec, "Este voucher já foi utilizado"));
        Add("ativoSpec", new Rule<Voucher>(ativoSpec, "Este voucher não está mais ativo"));
    }
}
