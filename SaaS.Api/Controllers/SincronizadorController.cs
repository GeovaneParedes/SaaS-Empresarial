using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SaaS.Data.Services;

namespace SaaS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SincronizadorController : ControllerBase
    {
        private readonly FirebirdSyncService _syncService;
        private readonly IMemoryCache _cache;

        public SincronizadorController(IMemoryCache cache)
        {
            _syncService = new FirebirdSyncService();
            _cache = cache;
        }

        [HttpGet("extrair-pagamentos")]
        public IActionResult ExtrairPagamentosDetalhados([FromQuery] string dataInicio, [FromQuery] string dataFim, [FromQuery] int lojaId = 1)
        {
            try
            {
                string cacheKey = $"ext_pag_{lojaId}_{dataInicio}_{dataFim}";
                if (_cache.TryGetValue(cacheKey, out object? cachedResult) && cachedResult != null)
                {
                    return Ok(cachedResult);
                }

                var dIni = DateTime.Parse(dataInicio);
                var dFim = DateTime.Parse(dataFim);

                string dbPath = "/opt/firebird/data/DADOS_CASADECARNEEXCELENCIA.FDB";
                if (lojaId == 2)
                {
                    dbPath = "/opt/firebird/data/PITSTOPDACARNE.FDB";
                }
                else if (lojaId == 3)
                {
                    dbPath = "/opt/firebird/data/CasaDeCarneCunhaFB50.FDB";
                }

                var cupons = _syncService.ExtrairVendasEFormasPagamentoCompleto(
                    "200.150.202.5",
                    dbPath,
                    "SYSDBA",
                    "***REMOVED***",
                    dIni,
                    dFim
                );

                // Cálculo consolidado de Taxas por Meio de Pagamento
                decimal totalVendido = cupons.Sum(c => c.TotalVenda);
                decimal totalPix = cupons.SelectMany(c => c.Pagamentos).Where(p => p.FormaPagamento.Contains("PIX")).Sum(p => p.Valor);
                decimal totalDebito = cupons.SelectMany(c => c.Pagamentos).Where(p => p.FormaPagamento.Contains("DEBITO")).Sum(p => p.Valor);
                decimal totalCredito = cupons.SelectMany(c => c.Pagamentos).Where(p => p.FormaPagamento.Contains("CREDITO")).Sum(p => p.Valor);
                decimal totalVoucher = cupons.SelectMany(c => c.Pagamentos).Where(p => p.FormaPagamento.Contains("VOUCHER")).Sum(p => p.Valor);
                decimal totalDinheiro = cupons.SelectMany(c => c.Pagamentos).Where(p => p.FormaPagamento.Contains("DINHEIRO")).Sum(p => p.Valor);
                
                decimal totalTaxasEstimadasMaquininha = cupons.SelectMany(c => c.Pagamentos).Sum(p => p.Valor - p.ValorLiquidoRecebido);

                var responseData = new
                {
                    Status = "Sucesso",
                    Periodo = $"{dIni:dd/MM/yyyy} ate {dFim:dd/MM/yyyy}",
                    TotalCupons = cupons.Count,
                    FaturamentoBruto = totalVendido,
                    ResumoPorMeioPagamento = new
                    {
                        Pix = totalPix,
                        Debito = totalDebito,
                        Credito = totalCredito,
                        VoucherAlimentacao = totalVoucher,
                        Dinheiro = totalDinheiro
                    },
                    CustoTaxasMaquininha = new
                    {
                        TotalPerdidoEmTaxas = totalTaxasEstimadasMaquininha,
                        ValorLiquidoQueCaiuNaConta = totalVendido - totalTaxasEstimadasMaquininha
                    },
                    AmostraCupons = cupons // Retorna a totalidade dos cupons do dia sem limite artificial
                };

                _cache.Set(cacheKey, responseData, TimeSpan.FromSeconds(45));
                return Ok(responseData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERRO EXTRACAO FIREBIRD] {ex}");
                return StatusCode(500, new { Erro = ex.Message, Detalhes = ex.ToString() });
            }
        }
    }
}
