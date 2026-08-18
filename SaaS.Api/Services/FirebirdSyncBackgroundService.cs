using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SaaS.Data.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SaaS.Api.Services
{
    public class FirebirdSyncBackgroundService : BackgroundService
    {
        private readonly ILogger<FirebirdSyncBackgroundService> _logger;
        private readonly FirebirdSyncService _syncService;
        private static readonly string PgConnStr = "Host=localhost;Database=acougue;Username=harrison;Password=felipemiguel";

        public FirebirdSyncBackgroundService(ILogger<FirebirdSyncBackgroundService> logger)
        {
            _logger = logger;
            _syncService = new FirebirdSyncService();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("🚀 [WORKER SEGUNDO PLANO] Inicializado serviço de sincronização assíncrona de Cupons Firebird.");

            // Aguarda 10 segundos antes do primeiro ciclo
            await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("🔄 [WORKER SEGUNDO PLANO] Executando varredura automatizada nas 3 lojas para o dia {Data}...", DateTime.Today.ToString("yyyy-MM-dd"));

                    // Loja 1: Excelência
                    ExecutarVarreduraUnidade(1, "/opt/firebird/data/DADOS_CASADECARNEEXCELENCIA.FDB");

                    // Loja 2: Pit Stop
                    ExecutarVarreduraUnidade(2, "/opt/firebird/data/PITSTOPDACARNE.FDB");

                    // Loja 3: Cunha
                    ExecutarVarreduraUnidade(3, "/opt/firebird/data/CasaDeCarneCunhaFB50.FDB");

                    _logger.LogInformation("✅ [WORKER SEGUNDO PLANO] Ciclo de sincronização de cupons finalizado com sucesso!");
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("⚠️ [WORKER SEGUNDO PLANO] Conexão remota indisponível neste ciclo: {Msg}", ex.Message);
                }

                // Executa o ciclo periodicamente a cada 15 minutos em segundo plano sem travar requisições
                await Task.Delay(TimeSpan.FromMinutes(15), stoppingToken);
            }
        }

        private void ExecutarVarreduraUnidade(int lojaId, string dbPath)
        {
            try
            {
                var cupons = _syncService.ExtrairVendasEFormasPagamentoCompleto(
                    "200.150.202.5",
                    dbPath,
                    "SYSDBA",
                    "***REMOVED***",
                    DateTime.Today,
                    DateTime.Today
                );

                if (cupons != null && cupons.Count > 0)
                {
                    _syncService.PersistirItensNoPostgres(PgConnStr, lojaId, DateTime.Today, cupons);
                    _logger.LogInformation("  ✔ [LOJA {LojaId}] {Qtd} cupons unitários sincronizados no PostgreSQL.", lojaId, cupons.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("  ⚠️ [LOJA {LojaId}] Firebird inacessível no momento: {Msg}", lojaId, ex.Message);
            }
        }
    }
}
