using Xunit;
using SaaS.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaaS.Tests
{
    public class VendasDiariasTests
    {
        [Fact]
        public void DeveCalcularSomatorioTotalGavetaEDinheiroComExatidao()
        {
            var vendas = new List<VendasDiariasResumoSaaS>
            {
                new VendasDiariasResumoSaaS
                {
                    Id = 1001,
                    LojaId = 1,
                    LojaNome = "EXCELENCIA",
                    Data = new DateTime(2026, 8, 1),
                    Dinheiro = 1500.50m,
                    PixVendido = 2300.00m,
                    CartaoVendido = 4500.80m,
                    Sangria = 300.00m,
                    TotalGaveta = 8301.30m
                },
                new VendasDiariasResumoSaaS
                {
                    Id = 1002,
                    LojaId = 2,
                    LojaNome = "PIT STOP",
                    Data = new DateTime(2026, 8, 1),
                    Dinheiro = 1200.00m,
                    PixVendido = 1800.00m,
                    CartaoVendido = 3200.00m,
                    Sangria = 150.00m,
                    TotalGaveta = 6200.00m
                }
            };

            decimal totalDinheiro = vendas.Sum(v => v.Dinheiro);
            decimal totalPix = vendas.Sum(v => v.PixVendido);
            decimal totalCartao = vendas.Sum(v => v.CartaoVendido);
            decimal totalSangria = vendas.Sum(v => v.Sangria);
            decimal totalGaveta = vendas.Sum(v => v.TotalGaveta);

            Assert.Equal(2700.50m, totalDinheiro);
            Assert.Equal(4100.00m, totalPix);
            Assert.Equal(7700.80m, totalCartao);
            Assert.Equal(450.00m, totalSangria);
            Assert.Equal(14501.30m, totalGaveta);
        }

        [Fact]
        public void DeveFiltrarVendasPorMesEDiaSemInconsistencia()
        {
            var lista = new List<VendasDiariasResumoSaaS>
            {
                new VendasDiariasResumoSaaS { Id = 1, Data = new DateTime(2026, 8, 10), LojaId = 1, TotalGaveta = 1000m },
                new VendasDiariasResumoSaaS { Id = 2, Data = new DateTime(2026, 8, 11), LojaId = 1, TotalGaveta = 2000m },
                new VendasDiariasResumoSaaS { Id = 3, Data = new DateTime(2026, 7, 10), LojaId = 1, TotalGaveta = 3000m },
            };

            var vendasAgosto = lista.Where(v => v.Data.Month == 8 && v.Data.Year == 2026).ToList();
            var vendaDia10 = lista.Where(v => v.Data.Month == 8 && v.Data.Year == 2026 && v.Data.Day == 10).ToList();

            Assert.Equal(2, vendasAgosto.Count);
            Assert.Equal(3000m, vendasAgosto.Sum(v => v.TotalGaveta));

            Assert.Single(vendaDia10);
            Assert.Equal(1000m, vendaDia10.First().TotalGaveta);
        }
    }
}
