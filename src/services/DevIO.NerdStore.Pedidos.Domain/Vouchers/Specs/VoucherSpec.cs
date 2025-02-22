﻿using DevIO.NerdStore.Core.Specification;
using System.Linq.Expressions;

namespace DevIO.NerdStore.Pedidos.Domain.Vouchers.Specs;

public class VoucherDataSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression() => voucher => voucher.DataValidade >= DateTime.Now;
}

public class VoucherQuantidadeSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression() => voucher => voucher.Quantidade > 0;
}

public class VoucherAtivoSpecification : Specification<Voucher>
{
    public override Expression<Func<Voucher, bool>> ToExpression() => voucher => voucher.Ativo && !voucher.Utilizado;
}