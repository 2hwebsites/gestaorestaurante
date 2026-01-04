namespace EstoqueLocal.Domain.Entities.Finance;

public enum TipoLancamento
{
    Entrada = 0,
    Saida = 1
}

public enum OrigemVenda
{
    ALaCarte = 0,
    BuffetFeijoada = 1,
    Bebidas = 2,
    Outros = 3
}

public enum FormaPagamento
{
    Dinheiro = 0,
    Pix = 1,
    CartaoCredito = 2,
    CartaoDebito = 3,
    Boleto = 4,
    Transferencia = 5,
    Outros = 6
}

public enum AlocacaoCusto
{
    BuffetFeijoada = 0,
    ALaCarte = 1,
    Bebidas = 2,
    Geral = 3,
    Administrativo = 4
}

public enum StatusContaPagar
{
    Aberta = 0,
    Paga = 1,
    Vencida = 2
}

public enum StatusContaReceber
{
    Aberta = 0,
    Recebida = 1,
    Atrasada = 2
}
