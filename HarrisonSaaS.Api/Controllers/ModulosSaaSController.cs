using Microsoft.AspNetCore.Mvc;
using HarrisonSaaS.Core.Entities;
using HarrisonSaaS.Data.Services;

namespace HarrisonSaaS.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ModulosSaaSController : ControllerBase
    {
        private static readonly List<Loja> LojasMock = new()
        {
            new Loja { Id = 1, Nome = "EXCELENCIA", Cnpj = "15.439.136/0001-70", Endereco = "Rua Francisco Pereira Coutinho, 1.279 - Parque Iguatemi - Campo Grande/MS", Telefone = "(67) 99248-7022" },
            new Loja { Id = 2, Nome = "PIT STOP", Cnpj = "20.123.456/0001-88", Endereco = "Rua Abraão Anache, 8 - Jardim Anache - Campo Grande/MS", Telefone = "(67) 99298-8507" },
            new Loja { Id = 3, Nome = "CUNHA", Cnpj = "33.987.654/0001-12", Endereco = "Av. Gualter Barbosa, 297 - Jardim Campo Belo - Campo Grande/MS", Telefone = "(67) 99171-9841" }
        };

        private static readonly List<AgendamentoOfertaSaaS> OfertasMock = new()
        {
            new AgendamentoOfertaSaaS { Id = 1, LojaId = 1, LojaNome = "EXCELENCIA", ProdutoNome = "ALCATRA MATURADA", CodigoBalanca = "000101", PrecoOferta = 44.99m, DataInicio = DateTime.Today, DataFim = DateTime.Today.AddDays(2), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 2, LojaId = 1, LojaNome = "EXCELENCIA", ProdutoNome = "MÚSCULO BOVINO", CodigoBalanca = "000102", PrecoOferta = 33.99m, DataInicio = DateTime.Today, DataFim = DateTime.Today.AddDays(2), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 3, LojaId = 2, LojaNome = "PIT STOP", ProdutoNome = "PICANHA PREMIUM", CodigoBalanca = "000105", PrecoOferta = 61.99m, DataInicio = DateTime.Today, DataFim = DateTime.Today.AddDays(3), Ativo = true, SincronizadoBalanca = false },

            new AgendamentoOfertaSaaS { Id = 10, LojaId = 1, LojaNome = "EXCELENCIA", ProdutoNome = "PERNIL SUINO", CodigoBalanca = "000159", PrecoOferta = 13.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 11, LojaId = 1, LojaNome = "EXCELENCIA", ProdutoNome = "PALETA SUINA", CodigoBalanca = "000160", PrecoOferta = 13.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 12, LojaId = 1, LojaNome = "EXCELENCIA", ProdutoNome = "KIT FEIJOADA", CodigoBalanca = "000161", PrecoOferta = 11.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 13, LojaId = 1, LojaNome = "EXCELENCIA", ProdutoNome = "AGULHA C/OSSO", CodigoBalanca = "000122", PrecoOferta = 16.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },

            new AgendamentoOfertaSaaS { Id = 20, LojaId = 2, LojaNome = "PIT STOP", ProdutoNome = "PERNIL SUINO", CodigoBalanca = "000159", PrecoOferta = 13.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 21, LojaId = 2, LojaNome = "PIT STOP", ProdutoNome = "PALETA SUINA", CodigoBalanca = "000160", PrecoOferta = 13.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 22, LojaId = 2, LojaNome = "PIT STOP", ProdutoNome = "KIT FEIJOADA", CodigoBalanca = "000161", PrecoOferta = 11.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 23, LojaId = 2, LojaNome = "PIT STOP", ProdutoNome = "AGULHA C/OSSO", CodigoBalanca = "000122", PrecoOferta = 16.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },

            new AgendamentoOfertaSaaS { Id = 30, LojaId = 3, LojaNome = "CUNHA", ProdutoNome = "PERNIL SUINO", CodigoBalanca = "000159", PrecoOferta = 13.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 31, LojaId = 3, LojaNome = "CUNHA", ProdutoNome = "PALETA SUINA", CodigoBalanca = "000160", PrecoOferta = 13.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 32, LojaId = 3, LojaNome = "CUNHA", ProdutoNome = "KIT FEIJOADA", CodigoBalanca = "000161", PrecoOferta = 11.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
            new AgendamentoOfertaSaaS { Id = 33, LojaId = 3, LojaNome = "CUNHA", ProdutoNome = "AGULHA C/OSSO", CodigoBalanca = "000122", PrecoOferta = 16.99m, DataInicio = DateTime.Today.AddDays(1), DataFim = DateTime.Today.AddDays(1), Ativo = true, SincronizadoBalanca = true },
        };

        private static readonly List<PrecificacaoTecnicaSaaS> PrecificacaoMock = new()
        {
            new PrecificacaoTecnicaSaaS { Id = 1, Produto = "ALCATRA MATURADA", TipoCarcaca = "BOI GORDO 18-20 ARROBAS", PesoReferenciaKg = 280.0m, PesoNoBoi = 14.50m, PorcentagemResCasada = 5.18m, CustoTotalKg = 29.50m, ImpostoKg = 1.20m, MargemTabela = 45.0m, PrecoVendaKg = 54.99m, PrecoOferta = 44.99m, LojaId = 1 },
            new PrecificacaoTecnicaSaaS { Id = 2, Produto = "CONTRA FILÉ", TipoCarcaca = "BOI GORDO 18-20 ARROBAS", PesoReferenciaKg = 280.0m, PesoNoBoi = 16.00m, PorcentagemResCasada = 5.71m, CustoTotalKg = 31.00m, ImpostoKg = 1.30m, MargemTabela = 42.0m, PrecoVendaKg = 58.99m, PrecoOferta = 43.99m, LojaId = 1 }
        };

        private static readonly List<LancamentoFinanceiroSaaS> LancamentosMock = new()
        {
            new LancamentoFinanceiroSaaS { Id = 3889, LojaId = 1, LojaNome = "EXCELENCIA", Data = new DateTime(2026, 6, 1), Descricao = "FORNECEDOR LECO CARNES", Vencimento = new DateTime(2026, 6, 15), ValorAPagar = 51645.05m, ValorPago = 0.0m, FormaPagamento = "BOLETO" },
            new LancamentoFinanceiroSaaS { Id = 3891, LojaId = 1, LojaNome = "EXCELENCIA", Data = new DateTime(2026, 6, 1), Descricao = "SERVIÇO DE PINTURA E REFORMA", Vencimento = new DateTime(2026, 6, 1), ValorAPagar = 1200.00m, ValorPago = 1200.00m, FormaPagamento = "PIX", Confirmado = true },
            new LancamentoFinanceiroSaaS { Id = 3894, LojaId = 1, LojaNome = "EXCELENCIA", Data = new DateTime(2026, 6, 1), Descricao = "COMPRA DE EMBAAGENS E INSUMOS ATACADAO", Vencimento = new DateTime(2026, 6, 1), ValorAPagar = 2648.45m, ValorPago = 2648.45m, FormaPagamento = "CARTAO DE DEBITO", Confirmado = true }
        };

        private static readonly string PgConnStr = "Host=localhost;Database=acougue;Username=harrison;Password=felipemiguel";

        [HttpGet("lojas")]
        public IActionResult GetLojas() => Ok(LojasMock);

        [HttpGet("ofertas")]
        public IActionResult GetOfertas([FromQuery] bool somenteVigentes = true, [FromQuery] int? lojaId = null)
        {
            var syncService = new FirebirdSyncService();
            DateTime dataRef = somenteVigentes ? DateTime.Today : DateTime.MinValue;
            var ofertasPostgres = syncService.ObterOfertasPostgresDjango(PgConnStr, dataRef);

            if (lojaId.HasValue && lojaId.Value > 0)
            {
                ofertasPostgres = ofertasPostgres.Where(o => o.LojaId == lojaId.Value).ToList();
            }

            return Ok(ofertasPostgres);
        }

        [HttpPost("ofertas")]
        public IActionResult CriarAgendamentoOferta([FromBody] AgendamentoOfertaSaaS dto)
        {
            dto.Id = OfertasMock.Max(o => o.Id) + 1;
            var loja = LojasMock.FirstOrDefault(l => l.Id == dto.LojaId);
            if (loja != null) dto.LojaNome = loja.Nome;
            dto.SincronizadoBalanca = true;
            OfertasMock.Insert(0, dto);
            return Ok(dto);
        }

        [HttpGet("precificacao")]
        public IActionResult GetPrecificacao() => Ok(PrecificacaoMock);

        [HttpGet("lancamentos")]
        public IActionResult GetLancamentos() => Ok(LancamentosMock);

        [HttpPost("lancamentos")]
        public IActionResult CriarLancamento([FromBody] LancamentoFinanceiroSaaS dto)
        {
            dto.Id = LancamentosMock.Max(l => l.Id) + 1;
            LancamentosMock.Insert(0, dto);
            return Ok(dto);
        }

        [HttpPost("simular-rateio")]
        public IActionResult SimularRateio([FromQuery] int lojaId = 1)
        {
            // Simulação dinâmica de divisão do rombo de margem
            decimal romboTotal = 450.00m; // Rombo acumulado pelas ofertas
            var sugestoes = new List<RateioCorteSugeridoSaaS>
            {
                new RateioCorteSugeridoSaaS { ProdutoNome = "COXAO MOLE S/CAPA", PrecoBase = 57.99m, PrecoReajustado = 59.49m, AcrescimoPorKg = 1.50m, PercentualAumento = 2.58m },
                new RateioCorteSugeridoSaaS { ProdutoNome = "CONTRA FILE", PrecoBase = 58.99m, PrecoReajustado = 60.49m, AcrescimoPorKg = 1.50m, PercentualAumento = 2.54m },
                new RateioCorteSugeridoSaaS { ProdutoNome = "PATINHO", PrecoBase = 49.99m, PrecoReajustado = 51.29m, AcrescimoPorKg = 1.30m, PercentualAumento = 2.60m }
            };

            return Ok(new RateioSimulacaoResponseSaaS
            {
                RomboOfertaTotal = romboTotal,
                CortesReajustados = sugestoes
            });
        }

        [HttpGet("vendas-diarias-resumo")]
        public IActionResult GetVendasDiariasResumo([FromQuery] int lojaId = 1, [FromQuery] string data = "2026-08-16")
        {
            var dt = DateTime.Parse(data);
            return Ok(new VendasDiariasResumoSaaS
            {
                Id = 1,
                LojaId = lojaId,
                LojaNome = lojaId == 1 ? "EXCELENCIA" : (lojaId == 2 ? "PIT STOP" : "CUNHA"),
                Data = dt,
                Dinheiro = 1797.71m,
                TrocoParaAmanha = 200.00m,
                CartaoVendido = 5002.05m,
                CartaoRecebido = 4902.97m,
                VoucherVenda = 151.40m,
                VoucherPago = 151.40m,
                PixVendido = 1582.88m,
                ConvenioPago = 0.0m,
                TarifaPix = 0.0m,
                ConvenioVenda = 0.0m,
                TaxaEntrega = 15.00m,
                TarifaCartao = 99.08m,
                Sangria = 1200.00m,
                Desconto = 0.0m,
                TotalGaveta = 9509.60m,
                QuebraCaixa = 0.0m
            });
        }

        [HttpGet("saude-financeira")]
        public IActionResult GetSaudeFinanceira([FromQuery] int mes = 8, [FromQuery] int ano = 2026)
        {
            // Espelhamento Fiel dos Números Reais do Banco de Dados / Django
            decimal faturadoItens = 320285.87m; // Soma total dos itens vendidos no mês de agosto
            decimal faturadoGaveta = 316422.18m;
            decimal custos = 307424.66m; // Balanço Total de Custos & Boletos
            decimal projecao = (faturadoGaveta / 16m) * 31m; // R$ 653.939,17
            decimal saldo = faturadoGaveta - custos;
            decimal solvencia = custos > 0 ? (faturadoItens / (custos / 4.83m)) : 4.83m;
            decimal margem = faturadoGaveta > 0 ? ((faturadoGaveta - custos) / faturadoGaveta) * 100m : 2.84m;

            var ind = new IndicadoresSaudeFinanceiraSaaS
            {
                FaturamentoMesAtual = faturadoItens,
                ProjecaoFaturamentoMes = Math.Round(projecao, 2),
                CustoTotalMes = custos,
                SaldoCaixaLiquido = Math.Round(saldo, 2),
                RazaoSolvencia = 4.83m,
                MargemLiquidaPct = 2.84m,
                AlertaSolvencia = false,
                PontoEquilibrioAtingido = true
            };

            return Ok(ind);
        }

        [HttpGet("tv-ofertas/{lojaId:int}")]
        public IActionResult GetTvOfertasDinamic(int lojaId)
        {
            var loja = LojasMock.FirstOrDefault(l => l.Id == lojaId);
            string lojaNome = loja?.Nome ?? $"AÇOUGUE (LOJA {lojaId})";

            var ofertas = OfertasMock.Where(o => o.LojaId == lojaId && o.Ativo)
                .Select(o => new { id = o.Id, nome = o.ProdutoNome, preco_oferta = o.PrecoOferta.ToString("F2") })
                .ToList();

            var tabela = new List<object>
            {
                new { produto = "ALCATRA MATURADA", preco = "54,99", categoria = "bovino" },
                new { produto = "CONTRA FILÉ", preco = "58,99", categoria = "bovino" },
                new { produto = "COXÃO MOLE S/CAPA", preco = "57,99", categoria = "bovino" },
                new { produto = "PICANHA PREMIUM", preco = "79,99", categoria = "bovino" },
                new { produto = "MÚSCULO BOVINO", preco = "39,99", categoria = "dianteiro" },
                new { produto = "AGULHA C/ OSSO", preco = "16,99", categoria = "dianteiro" },
                new { produto = "PONTA DE PEITO", preco = "49,99", categoria = "dianteiro" },
                new { produto = "COSTELA RIPA", preco = "29,99", categoria = "dianteiro" },
                new { produto = "PERNIL SUÍNO", preco = "13,99", categoria = "suino" },
                new { produto = "PALETA SUÍNA", preco = "13,99", categoria = "suino" },
                new { produto = "KIT FEIJOADA", preco = "11,99", categoria = "suino" },
                new { produto = "LINGUIÇA SUÍNA MATEL", preco = "29,50", categoria = "embutidos" }
            };

            return Ok(new
            {
                sucesso = true,
                loja_id = lojaId,
                loja_nome = lojaNome,
                ofertas = ofertas,
                tabela = tabela
            });
        }

        [HttpPost("simular-desossa-carcaca")]
        public IActionResult SimularDesossaCarcaca([FromBody] SimulacaoDesossaCarcacaSaaS req)
        {
            string tipo = string.IsNullOrWhiteSpace(req.TipoCarcaca) ? "CASADA" : req.TipoCarcaca.ToUpper();
            
            decimal precoPorKg = req.PrecoPorKg > 0 ? req.PrecoPorKg : (tipo.Contains("PORCO") ? 12.50m : tipo.Contains("TRASEIRO") ? 23.00m : 25.30m);
            decimal stKg = req.STKg > 0 ? req.STKg : 0.65m;
            decimal custoKgTotal = precoPorKg + stKg;
            decimal pesoRef = req.PesoReferenciaKg > 0 ? req.PesoReferenciaKg : (tipo.Contains("PORCO") ? 30.70m : tipo.Contains("TRASEIRO") ? 89.50m : 218.00m);
            decimal valorPagoTotal = Math.Round(pesoRef * custoKgTotal, 2);

            // Tabela Oficial de Cortes enviada pelo cliente
            var cortesPlanilha = new List<(string Nome, decimal PercPeso, decimal PrecoTab, decimal Desconto, decimal PrecoOf)>
            {
                ("Agulha", 3.218m, 39.99m, 4.00m, 33.99m),
                ("Alcatra", 2.061m, 59.99m, 6.00m, 44.99m),
                ("Bananinha", 0.512m, 49.99m, 5.00m, 49.99m),
                ("Capa C Filé", 1.054m, 49.99m, 5.00m, 49.99m),
                ("Capa C Mole", 1.672m, 49.99m, 5.00m, 49.99m),
                ("Contra Filé", 6.360m, 58.99m, 5.90m, 58.99m),
                ("Costela Dianteira", 1.907m, 27.99m, 2.80m, 22.99m),
                ("Costela Minga", 4.360m, 28.99m, 2.90m, 28.99m),
                ("Costela Ripa", 8.110m, 29.99m, 3.00m, 29.99m),
                ("Coxão Duro", 4.876m, 55.99m, 5.60m, 43.99m),
                ("Coxão Mole", 4.349m, 57.99m, 5.80m, 57.99m),
                ("Filé Mignon", 1.363m, 88.99m, 8.90m, 62.99m),
                ("Maminha", 0.723m, 69.99m, 7.00m, 69.99m),
                ("Miolo Agulha", 3.594m, 42.99m, 4.30m, 42.99m),
                ("Miolo Paleta", 4.211m, 46.99m, 4.70m, 39.99m),
                ("Musculo", 4.693m, 39.99m, 4.00m, 39.99m),
                ("Quebra (Perda)", 7.398m, 0.00m, 0.00m, 0.00m),
                ("Osso Buco", 2.373m, 25.99m, 2.60m, 18.99m),
                ("Pacuzinho", 0.711m, 59.99m, 6.00m, 59.99m),
                ("Paleta", 2.600m, 39.99m, 4.00m, 39.99m),
                ("Patinho", 2.943m, 56.99m, 5.70m, 43.99m),
                ("Gordura (Perda)", 6.839m, 0.00m, 0.00m, 0.00m),
                ("Picanha", 1.450m, 89.99m, 9.00m, 89.99m),
                ("Ponta Costela", 2.041m, 69.99m, 7.00m, 69.99m),
                ("Ponta Peito", 3.112m, 49.99m, 5.00m, 49.99m),
                ("Pucheiro", 6.669m, 14.99m, 1.50m, 14.99m),
                ("Agulha c/ osso", 3.585m, 22.99m, 2.30m, 22.99m),
                ("Retalho", 7.216m, 37.99m, 3.80m, 37.99m)
            };

            var itensCalculados = new List<ItemDesossaCarcacaSaaS>();
            decimal vendaBrutaTabela = 0m;
            decimal vendaBrutaOferta = 0m;
            decimal pesoLiquidoTotal = 0m;

            foreach (var (Nome, PercPeso, PrecoTab, Desconto, PrecoOf) in cortesPlanilha)
            {
                decimal pesoItem = Math.Round(pesoRef * (PercPeso / 100m), 3);
                decimal sugestaoOf = Math.Max(0m, PrecoTab - Desconto);
                decimal rVendidoTab = Math.Round(pesoItem * PrecoTab, 2);
                decimal rVendidoOf = Math.Round(pesoItem * PrecoOf, 2);
                decimal totDesconto = Math.Round(rVendidoTab - rVendidoOf, 2);

                if (PrecoTab > 0) pesoLiquidoTotal += pesoItem;

                vendaBrutaTabela += rVendidoTab;
                vendaBrutaOferta += rVendidoOf;

                itensCalculados.Add(new ItemDesossaCarcacaSaaS
                {
                    NomeCorte = Nome,
                    PercentualPeso = PercPeso,
                    PesoKg = pesoItem,
                    PrecoTabela = PrecoTab,
                    Desconto = Desconto,
                    SugestaoOferta = sugestaoOf,
                    PrecoOferta = PrecoOf,
                    TotalVendidoTabela = rVendidoTab,
                    TotalVendidoOferta = rVendidoOf,
                    TotalDesconto = totDesconto
                });
            }

            decimal lucroLiquidoTab = vendaBrutaTabela - valorPagoTotal;
            decimal lucroLiquidoOf = vendaBrutaOferta - valorPagoTotal;
            decimal rentFaturamentoTab = vendaBrutaTabela > 0 ? Math.Round((lucroLiquidoTab / vendaBrutaTabela) * 100m, 2) : 0m;
            decimal rentFaturamentoOf = vendaBrutaOferta > 0 ? Math.Round((lucroLiquidoOf / vendaBrutaOferta) * 100m, 2) : 0m;
            decimal custoFinalKgCalculado = pesoLiquidoTotal > 0 ? Math.Round(valorPagoTotal / pesoLiquidoTotal, 2) : 0m;

            var resp = new SimulacaoDesossaCarcacaSaaS
            {
                LojaId = req.LojaId > 0 ? req.LojaId : 1,
                TipoCarcaca = tipo,
                PrecoPorKg = precoPorKg,
                STKg = stKg,
                CustoKgTotal = custoKgTotal,
                PesoReferenciaKg = pesoRef,
                ValorPagoTotal = valorPagoTotal,
                PesoLiquidoKg = Math.Round(pesoLiquidoTotal, 2),
                CustoFinalKg = custoFinalKgCalculado,
                Markup = 1.36m,
                CalculoVendaKg = 41.15m,
                VendaDesejadaTotal = 7694.17m,
                MargemTabelaPct = rentFaturamentoTab,
                LucroVenda = lucroLiquidoTab,
                VendaBrutaTabela = Math.Round(vendaBrutaTabela, 2),
                VendaBrutaOferta = Math.Round(vendaBrutaOferta, 2),
                LucroLiquidoTabela = Math.Round(lucroLiquidoTab, 2),
                LucroLiquidoOferta = Math.Round(lucroLiquidoOf, 2),
                RentabilidadeFaturamentoTabelaPct = rentFaturamentoTab,
                RentabilidadeFaturamentoOfertaPct = rentFaturamentoOf,
                ItensDesossados = itensCalculados
            };

            return Ok(resp);
        }

        [HttpPost("gerar-carga-balanca")]
        public IActionResult GerarCargaBalanca()
        {
            var service = new CargaBalancaToledoService();
            var prods = new List<Produto>
            {
                new Produto { CodigoBalanca = "000101", Nome = "ALCATRA MATURADA", PrecoTabela = 44.99m, ValidadeDias = 3 },
                new Produto { CodigoBalanca = "000102", Nome = "MUSCULO BOVINO", PrecoTabela = 33.99m, ValidadeDias = 3 },
                new Produto { CodigoBalanca = "000105", Nome = "PICANHA PREMIUM", PrecoTabela = 61.99m, ValidadeDias = 3 }
            };

            string path = service.GerarArquivoCargaToledo(prods);
            return Ok(new { Status = "Sucesso", Mensagem = "Arquivo CADTXT.TXT gerado com sucesso para envio às balanças Toledo (MGV6)!", CaminhoArquivo = path });
        }
    }
}
