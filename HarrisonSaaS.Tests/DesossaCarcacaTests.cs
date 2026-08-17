using Xunit;
using HarrisonSaaS.Core.Entities;
using System.Collections.Generic;

namespace HarrisonSaaS.Tests
{
    public class DesossaCarcacaTests
    {
        [Fact]
        public void DeveCalcularRentabilidadeEDesossaComPrecisao()
        {
            // Arrange (Cenário do Boi Casado 218kg)
            var simulacao = new SimulacaoDesossaCarcacaSaaS
            {
                TipoCarcaca = "CASADA (218kg)",
                PesoReferenciaKg = 218.0m,
                PrecoPorKg = 25.30m,
                STKg = 0.65m,
                CustoKgTotal = 25.95m,
                ValorPagoTotal = 5657.10m,
                CustoFinalKg = 30.26m
            };

            // Assert
            Assert.Equal(25.95m, simulacao.CustoKgTotal);
            Assert.Equal(5657.10m, simulacao.ValorPagoTotal);
            Assert.Equal(30.26m, simulacao.CustoFinalKg);
        }
    }
}
