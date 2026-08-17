using System;
using System.Collections.Generic;

namespace HarrisonSaaS.Core.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; } = string.Empty;
        public string RazaoSocial { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public string EmailContato { get; set; } = string.Empty;
        public string TelefoneWhatsApp { get; set; } = string.Empty;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool Ativo { get; set; } = true;
        public string SchemaName { get; set; } = "public";
    }

    public class UsuarioSaaS
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string TenantNome { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SenhaHash { get; set; } = string.Empty;
        public string Role { get; set; } = "DONO_TENANT"; // ADMIN_MASTER, DONO_TENANT, OPERADOR_LOJA
        public bool Ativo { get; set; } = true;
        public DateTime DataUltimoAcesso { get; set; } = DateTime.Now;
    }

    public class PlanoAssinaturaSaaS
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "PRO"; // STARTER, PRO, ENTERPRISE
        public decimal ValorMensal { get; set; } = 599.00m;
        public int LimiteLojas { get; set; } = 3;
        public bool ModuloDesossaLiberado { get; set; } = true;
        public bool AuditoriaTEFLiberada { get; set; } = true;
        public bool DRELiberado { get; set; } = true;
    }

    public class AssinaturaTenantSaaS
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string TenantNome { get; set; } = string.Empty;
        public int PlanoId { get; set; }
        public string PlanoNome { get; set; } = "PRO";
        public decimal ValorMensalidad { get; set; } = 599.00m;
        public DateTime DataInicio { get; set; } = DateTime.Today;
        public DateTime DataVencimento { get; set; } = DateTime.Today.AddDays(30);
        public string StatusAssinatura { get; set; } = "ATIVA"; // ATIVA, PENDENTE, SUSPENSA_ATRASO, CANCELADA
        public string ChavePixAsaas { get; set; } = string.Empty;
        public string GatewayId { get; set; } = string.Empty;
    }

    public class TaxaContratadaAdquirenteSaaS
    {
        public int Id { get; set; }
        public string AdquirenteNome { get; set; } = "STONE"; // STONE, PAGBANK, CIELO, REDE, TICKET, SODEXO
        public string Modalidade { get; set; } = "DEBITO"; // DEBITO, CREDITO_AVISTA, CREDITO_PARCELADO, VOUCHER_VR
        public decimal TaxaContratadaPct { get; set; } = 1.29m;
        public int DiasRecebimento { get; set; } = 1;
    }

    public class TransacaoTEFAuditadaSaaS
    {
        public int Id { get; set; }
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public long CupomId { get; set; }
        public DateTime DataHora { get; set; }
        public string Adquirente { get; set; } = "STONE";
        public string Bandeira { get; set; } = "MASTERCARD";
        public string Modalidade { get; set; } = "DEBITO";
        public decimal ValorVendaBruto { get; set; }
        public decimal TaxaContratadaPct { get; set; }
        public decimal TaxaEfetivaCobradaPct { get; set; }
        public decimal ValorTaxaContratada { get; set; }
        public decimal ValorTaxaCobrada { get; set; }
        public decimal ValorLiquidoEsperado { get; set; }
        public decimal ValorLiquidoRecebido { get; set; }
        public decimal PrejuizoTaxaIncorreta { get; set; }
        public bool DivergenciaDetectada { get; set; }
        public string StatusAuditoria { get; set; } = "AUDITADO_OK"; // AUDITADO_OK, TAXA_ABUSIVA_DETECTADA, NAO_CONCILIADO
    }

    public class ResumoAuditoriaTEFSaaS
    {
        public decimal TotalVendasProcessadas { get; set; }
        public decimal TotalTaxasContratadas { get; set; }
        public decimal TotalTaxasCobradasEfetivas { get; set; }
        public decimal PrejuizoTotalDetectado { get; set; }
        public int TransacoesAuditadasCount { get; set; }
        public int TransacoesComDivergenciaCount { get; set; }
        public List<TransacaoTEFAuditadaSaaS> DivergenciasCriticas { get; set; } = new();
    }

    public class ItemSugestaoCompraSaaS
    {
        public string ProdutoNome { get; set; } = string.Empty;
        public string Unidade { get; set; } = "KG";
        public decimal MediaVendaDiariaKg { get; set; }
        public decimal PrevisaoVendaFimDeSemanaKg { get; set; } // Sexta, Sábado e Domingo
        public decimal EstoqueAtualEstimadoKg { get; set; }
        public decimal SugestaoCompraKg { get; set; }
        public decimal PrecoCustoEstimadoKg { get; set; }
        public decimal ValorTotalEstimadoCompra { get; set; }
        public string NivelUrgencia { get; set; } = "CRITICO"; // CRITICO, MODERADO, ESTAVEL
    }

    public class SugestaoCompraPreditivaSaaS
    {
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public DateTime DataGeracao { get; set; } = DateTime.Now;
        public DateTime PeriodoPrevisaoInicio { get; set; }
        public DateTime PeriodoPrevisaoFim { get; set; }
        public decimal CustoTotalEstimadoPedidos { get; set; }
        public int QtdCarcacasBoiSugestao { get; set; }
        public decimal PesoTotalBoiSugestaoKg { get; set; }
        public List<ItemSugestaoCompraSaaS> ItensRecomendados { get; set; } = new();
    }

    public class ItemCardapioDigitalSaaS
    {
        public int Id { get; set; }
        public string Categoria { get; set; } = "BOVINO NOBRE"; // BOVINO NOBRE, DIA A DIA, SUINO, LINGUICAS, CHURRASCO
        public string NomeCorte { get; set; } = string.Empty;
        public decimal PrecoNormalKg { get; set; }
        public decimal PrecoOfertaKg { get; set; }
        public bool EmOferta { get; set; }
        public string FotoUrl { get; set; } = string.Empty;
        public bool EmEstoque { get; set; } = true;
    }

    public class MensagemWhatsappCotacaoSaaS
    {
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public string NumeroClienteWhatsapp { get; set; } = string.Empty;
        public string TextoFormatadoWhatsapp { get; set; } = string.Empty;
        public DateTime DataHoraEnvio { get; set; } = DateTime.Now;
        public string LinkCardapioDigital { get; set; } = string.Empty;
    }

    public class ItemCurvaABCSaaS
    {
        public string ProdutoNome { get; set; } = string.Empty;
        public decimal QuantidadeKgVendida { get; set; }
        public decimal FaturamentoTotal { get; set; }
        public decimal PercentualDoFaturamento { get; set; }
        public decimal PercentualAcumulado { get; set; }
        public string Classe { get; set; } = "A"; // CLASSE A (80% Faturamento), CLASSE B (15%), CLASSE C (5%)
    }

    public class DREGerencialSaaS
    {
        public int LojaId { get; set; }
        public string LojaNome { get; set; } = string.Empty;
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public decimal ReceitaBrutaVendas { get; set; }
        public decimal ImpostosETaxasAdquirentes { get; set; }
        public decimal ReceitaLiquidaVendas { get; set; }
        public decimal CMV_CustoMercadoriaVendida { get; set; }
        public decimal LucroBruto { get; set; }
        public decimal MargemBrutaPct { get; set; }
        public decimal DespesasOperacionaisOpEx { get; set; }
        public decimal EBITDA { get; set; }
        public decimal MargemEBITDAPct { get; set; }
        public decimal LucroLiquido { get; set; }
        public decimal MargemLiquidaPct { get; set; }
        public List<ItemCurvaABCSaaS> CurvaABC { get; set; } = new();
    }

    public class Loja
    {
        public int Id { get; set; }
        public int TenantId { get; set; } = 1;
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
