﻿namespace DevIO.NerdStore.Pagamentos.NerdsPag;

public enum TransactionStatus
{
    Authorized = 1,
    Paid,
    Refused,
    Chargedback,
    Cancelled
}