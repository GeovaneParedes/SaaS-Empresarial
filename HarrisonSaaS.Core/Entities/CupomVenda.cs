namespace HarrisonSaaS.Core.Entities
{
    public class CupomVenda
    {
        public long Id { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public DateTime DataHora { get; set; }
        public decimal TotalVenda { get; set; }
        public decimal TotalDesconto { get; set; }
        public List<ItemCupom> Itens { get; set; } = new();
        public List<PagamentoCupom> Pagamentos { get; set; } = new();
    }

    public class ItemCupom
    {
        public long Id { get; set; }
        public string ProdutoCodigo { get; set; } = string.Empty;
        public string ProdutoDescricao { get; set; } = string.Empty;
        public string Unidade { get; set; } = string.Empty;
        public decimal Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public bool EmOferta { get; set; }
    }

    public class PagamentoCupom
    {
        public long Id { get; set; }
        public string FormaPagamento { get; set; } = string.Empty; // Ex: PIX, CARTAO DE DEBITO, CARTAO DE CREDITO, VOUCHER, DINHEIRO
        public decimal Valor { get; set; }
        public decimal Troco { get; set; }
        public string? Bandeira { get; set; } // Ex: VISA, MASTERCARD, ELO, VR, SODEXO, ALELO
        public string? TipoCartao { get; set; } // Ex: DEBITO, CREDITO, VOUCHER
        public decimal TaxaPercentualEstimada { get; set; } // Taxa da maquininha
        public decimal ValorLiquidoRecebido { get; set; } // Valor descontado a taxa
    }
}
