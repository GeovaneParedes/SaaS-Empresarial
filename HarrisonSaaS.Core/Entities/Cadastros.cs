using System;
using System.Collections.Generic;

namespace HarrisonSaaS.Core.Entities
{
    public class Loja
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Endereco { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
    }

    public class Produto
    {
        public int Id { get; set; }
        public string CodigoBalanca { get; set; } = string.Empty; // Código PLU da Balança (Ex: 000123)
        public string Nome { get; set; } = string.Empty;
        public string Unidade { get; set; } = "KG";
        public decimal PrecoTabela { get; set; }
        public decimal PrecoCusto { get; set; }
        public int ValidadeDias { get; set; } = 3;
    }

    public class AgendamentoOfertaSaaS
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public int ProdutoId { get; set; }
        public string ProdutoNome { get; set; } = string.Empty;
        public string CodigoBalanca { get; set; } = string.Empty;
        public decimal PrecoOferta { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Ativo { get; set; } = true;
        public bool SincronizadoBalanca { get; set; } = false;
    }

    public class PrecificacaoTecnicaSaaS
    {
        public int Id { get; set; }
        public string Produto { get; set; } = string.Empty;
        public string TipoCarcaca { get; set; } = string.Empty;
        public decimal PesoReferenciaKg { get; set; }
        public decimal PesoNoBoi { get; set; }
        public decimal PorcentagemResCasada { get; set; }
        public decimal CustoTotalKg { get; set; }
        public decimal ImpostoKg { get; set; }
        public decimal MargemTabela { get; set; }
        public decimal PrecoVendaKg { get; set; }
        public decimal PrecoOferta { get; set; }
        public int LojaId { get; set; }
    }

    public class LancamentoFinanceiroSaaS
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public DateTime? Vencimento { get; set; }
        public decimal ValorAPagar { get; set; }
        public decimal ValorPago { get; set; }
        public string FormaPagamento { get; set; } = "BOLETO";
        public bool Confirmado { get; set; }
    }

    public class VendasDiariasResumoSaaS
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public decimal Dinheiro { get; set; }
        public decimal TrocoParaAmanha { get; set; }
        public decimal CartaoVendido { get; set; }
        public decimal CartaoRecebido { get; set; }
        public decimal VoucherVenda { get; set; }
        public decimal VoucherPago { get; set; }
        public decimal PixVendido { get; set; }
        public decimal ConvenioPago { get; set; }
        public decimal TarifaPix { get; set; }
        public decimal ConvenioVenda { get; set; }
        public decimal TaxaEntrega { get; set; }
        public decimal TarifaCartao { get; set; }
        public decimal Sangria { get; set; }
        public decimal Desconto { get; set; }
        public decimal TotalGaveta { get; set; }
        public decimal QuebraCaixa { get; set; }
    }

    public class CategoriaFinanceiraSaaS
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
    }

    public class RateioCorteSugeridoSaaS
    {
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public decimal PrecoReajustado { get; set; }
        public decimal AcrescimoPorKg { get; set; }
        public decimal PercentualAumento { get; set; }
    }

    public class RateioSimulacaoResponseSaaS
    {
        public decimal RomboOfertaTotal { get; set; }
        public List<RateioCorteSugeridoSaaS> CortesReajustados { get; set; } = new();
    }

    public class IndicadoresSaudeFinanceiraSaaS
    {
        public decimal FaturamentoMesAtual { get; set; }
        public decimal ProjecaoFaturamentoMes { get; set; }
        public decimal CustoTotalMes { get; set; }
        public decimal SaldoCaixaLiquido { get; set; }
        public decimal RazaoSolvencia { get; set; }
        public decimal MargemLiquidaPct { get; set; }
        public bool AlertaSolvencia { get; set; }
        public bool PontoEquilibrioAtingido { get; set; }
    }

    public class ItemDesossaCarcacaSaaS
    {
        public string NomeCorte { get; set; } = string.Empty;
        public decimal PercentualPeso { get; set; }
        public decimal PesoKg { get; set; }
        public decimal PrecoTabela { get; set; }
        public decimal Desconto { get; set; }
        public decimal SugestaoOferta { get; set; }
        public decimal PrecoOferta { get; set; }
        public decimal TotalVendidoTabela { get; set; }
        public decimal TotalVendidoOferta { get; set; }
        public decimal TotalDesconto { get; set; }
    }

    public class SimulacaoDesossaCarcacaSaaS
    {
        public int LojaId { get; set; }
        public string TipoCarcaca { get; set; } = "CASADA";
        public decimal PrecoPorKg { get; set; } = 25.30m;
        public decimal STKg { get; set; } = 0.65m;
        public decimal CustoKgTotal { get; set; } = 25.95m;
        public decimal PesoReferenciaKg { get; set; } = 218.00m;
        public decimal ValorPagoTotal { get; set; } = 5657.48m;
        public decimal PesoLiquidoKg { get; set; } = 186.96m;
        public decimal CustoFinalKg { get; set; } = 30.26m;
        public decimal Markup { get; set; } = 1.36m;
        public decimal CalculoVendaKg { get; set; } = 41.15m;
        public decimal VendaDesejadaTotal { get; set; } = 7694.17m;
        public decimal MargemTabelaPct { get; set; } = 25.27m;
        public decimal LucroVenda { get; set; } = 1913.51m;
        public decimal VendaBrutaTabela { get; set; } = 7570.99m;
        public decimal VendaBrutaOferta { get; set; } = 7280.96m;
        public decimal LucroLiquidoTabela { get; set; } = 1913.51m;
        public decimal LucroLiquidoOferta { get; set; } = 1623.48m;
        public decimal RentabilidadeFaturamentoTabelaPct { get; set; } = 25.27m;
        public decimal RentabilidadeFaturamentoOfertaPct { get; set; } = 22.30m;
        public List<ItemDesossaCarcacaSaaS> ItensDesossados { get; set; } = new();
    }
}
