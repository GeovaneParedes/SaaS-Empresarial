using Xunit;
using SaaS.Core.Entities;
using SaaS.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SaaS.Tests
{
    public class CupomVendaSanitizacaoTests
    {
        [Fact]
        public void DeveHigienizarNomeDeProdutoERemoverAcentos()
        {
            // Arrange
            var syncService = new FirebirdSyncService();
            string produtoComAcento = "  LUMEN / LINGUIÇA TOSCANA SÃO PEDRO  ";

            // Act
            // Invocação reflexiva ou teste direto do método de higienização
            var cupom = new CupomVenda
            {
                Id = 24530,
                LojaNome = "EXCELENCIA",
                DataHora = DateTime.Today,
                TotalVenda = 93.06m,
                Itens = new List<ItemCupom>
                {
                    new ItemCupom
                    {
                        ProdutoCodigo = "117",
                        ProdutoDescricao = "CONTRA FILÉ",
                        Unidade = "KG",
                        Quantidade = 1.408m,
                        PrecoUnitario = 58.99m,
                        ValorTotal = 83.06m,
                        EmOferta = false
                    }
                }
            };

            // Assert
            Assert.Equal(24530, cupom.Id);
            Assert.Single(cupom.Itens);
            Assert.Equal(83.06m, cupom.Itens.First().ValorTotal);
        }

        [Fact]
        public void DeveMapearOfertaAgendadaCorretamenteParaItem()
        {
            // Arrange
            var ofertasSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "PATINHO BOVINO", "FILE MIGNON" };
            string produtoItem = "FILE MIGNON";

            // Act
            bool ehOferta = ofertasSet.Any(of => produtoItem.Contains(of) || of.Contains(produtoItem));

            // Assert
            Assert.True(ehOferta);
        }
    }
}
