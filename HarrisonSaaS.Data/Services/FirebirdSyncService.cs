using System;
using System.Collections.Generic;
using System.Data;
using FirebirdSql.Data.FirebirdClient;
using HarrisonSaaS.Core.Entities;

namespace HarrisonSaaS.Data.Services
{
    public class FirebirdSyncService
    {
        public List<CupomVenda> ExtrairVendasEFormasPagamentoCompleto(string ip, string pathDb, string usuario, string senha, DateTime dataInicio, DateTime dataFim)
        {
            var listaCupons = new List<CupomVenda>();
            string connString = $"User={usuario};Password={senha};Database={pathDb};DataSource={ip};Port=3050;Charset=NONE;";

            using (var conn = new FbConnection(connString))
            {
                conn.Open();

                // 1. Extração dos Cupons Masters
                string sqlMaster = @"
                    SELECT 
                        codigo, data_emissao, subtotal, desconto, total
                    FROM vendas_master
                    WHERE situacao = 'F'
                      AND CAST(data_emissao AS DATE) BETWEEN @DataIni AND @DataFim
                    ORDER BY codigo DESC;";

                var mapCupons = new Dictionary<long, CupomVenda>();

                using (var cmd = new FbCommand(sqlMaster, conn))
                {
                    cmd.Parameters.AddWithValue("@DataIni", dataInicio.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@DataFim", dataFim.ToString("yyyy-MM-dd"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long id = Convert.ToInt64(reader["codigo"]);
                            var cupom = new CupomVenda
                            {
                                Id = id,
                                DataHora = Convert.ToDateTime(reader["data_emissao"]),
                                TotalVenda = Convert.ToDecimal(reader["total"]),
                                TotalDesconto = Convert.ToDecimal(reader["desconto"])
                            };
                            mapCupons[id] = cupom;
                            listaCupons.Add(cupom);
                        }
                    }
                }

                // 2. Extração Detalhada das Formas de Pagamento (PIX, DÉBITO, CRÉDITO, VOUCHER)
                string sqlPagamentos = @"
                    SELECT 
                        vf.vendas_master AS CupomId,
                        fp.descricao AS FormaPagamento,
                        vf.valor AS Valor,
                        vf.troco AS Troco,
                        vf.nome_administradora AS Bandeira,
                        vf.debito_credito AS TipoCartao
                    FROM vendas_fpg vf
                    LEFT JOIN forma_pagamento fp ON fp.codigo = vf.id_forma
                    JOIN vendas_master vm ON vm.codigo = vf.vendas_master
                    WHERE vm.situacao = 'F'
                      AND CAST(vm.data_emissao AS DATE) BETWEEN @DataIni AND @DataFim;";

                using (var cmd = new FbCommand(sqlPagamentos, conn))
                {
                    cmd.Parameters.AddWithValue("@DataIni", dataInicio.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@DataFim", dataFim.ToString("yyyy-MM-dd"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long cupomId = Convert.ToInt64(reader["CupomId"]);
                            if (mapCupons.TryGetValue(cupomId, out var cupom))
                            {
                                string formaStr = SanitizarTextoCompleto(reader["FormaPagamento"]?.ToString() ?? "OUTROS");
                                decimal valor = Convert.ToDecimal(reader["Valor"]);
                                string? tipoCartao = reader["TipoCartao"]?.ToString();

                                // Cálculo Inteligente de Taxas Estimadas por Meio de Pagamento
                                decimal taxaEstimada = 0.0m;
                                if (formaStr.Contains("PIX")) taxaEstimada = 0.00m; // 0% Pix
                                else if (formaStr.Contains("DEBITO") || tipoCartao == "D") taxaEstimada = 1.29m; // ~1.29% Débito
                                else if (formaStr.Contains("CREDITO") || tipoCartao == "C") taxaEstimada = 2.49m; // ~2.49% Crédito
                                else if (formaStr.Contains("VOUCHER") || formaStr.Contains("TICKET") || formaStr.Contains("ALIMENTACAO")) taxaEstimada = 4.50m; // ~4.50% Voucher VR/Sodexo

                                decimal valorLiquido = valor * (1 - (taxaEstimada / 100m));

                                decimal trocoVal = 0.0m;
                                if (reader["Troco"] != DBNull.Value && reader["Troco"] != null)
                                {
                                    trocoVal = Convert.ToDecimal(reader["Troco"]);
                                }

                                cupom.Pagamentos.Add(new PagamentoCupom
                                {
                                    FormaPagamento = formaStr,
                                    Valor = valor,
                                    Troco = trocoVal,
                                    Bandeira = reader["Bandeira"] != DBNull.Value ? reader["Bandeira"]?.ToString() : null,
                                    TipoCartao = reader["TipoCartao"] != DBNull.Value ? reader["TipoCartao"]?.ToString() : null,
                                    TaxaPercentualEstimada = taxaEstimada,
                                    ValorLiquidoRecebido = Math.Round(valorLiquido, 2)
                                });
                            }
                        }
                    }
                }
                // 3. Extração dos Itens de Produtos Comprados por Cupom
                string sqlItens = @"
                    SELECT 
                        vd.fkvenda AS CupomId,
                        COALESCE(p.codigo, vd.id_produto) AS ProdutoCodigo,
                        COALESCE(p.descricao, 'PRODUTO DIVERSO') AS ProdutoDescricao,
                        COALESCE(p.unidade, 'UN') AS Unidade,
                        vd.qtd AS Quantidade,
                        vd.preco AS PrecoUnitario,
                        vd.total AS ValorTotal
                    FROM vendas_detalhe vd
                    LEFT JOIN produto p ON p.codigo = vd.id_produto
                    JOIN vendas_master vm ON vm.codigo = vd.fkvenda
                    WHERE vm.situacao = 'F'
                      AND (vd.situacao IS NULL OR vd.situacao <> 'C')
                      AND vd.total > 0
                      AND CAST(vm.data_emissao AS DATE) BETWEEN @DataIni AND @DataFim;";

                using (var cmd = new FbCommand(sqlItens, conn))
                {
                    cmd.Parameters.AddWithValue("@DataIni", dataInicio.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@DataFim", dataFim.ToString("yyyy-MM-dd"));

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            long cupomId = Convert.ToInt64(reader["CupomId"]);
                            if (mapCupons.TryGetValue(cupomId, out var cupom))
                            {
                                string unidRaw = reader["Unidade"]?.ToString()?.Trim()?.ToUpper() ?? "KG";
                                string unidPadr = (unidRaw == "KILO" || unidRaw == "KG.") ? "KG" : (unidRaw == "UNIDA" || unidRaw == "UNIDADE") ? "UN" : unidRaw;

                                string codProd = reader["ProdutoCodigo"]?.ToString()?.Trim() ?? "0";
                                string descRaw = reader["ProdutoDescricao"]?.ToString();
                                string descProcessada = SanitizarTextoCompleto(descRaw);

                                // Mapeamento Inteligente de Códigos Órfãos do ERP Firebird sem cadastro
                                if (descProcessada == "PRODUTO DIVERSO")
                                {
                                    if (codProd == "15") descProcessada = "CARVAO 2.5KG";
                                    else if (codProd == "117") descProcessada = "PALETA BOVINA";
                                    else descProcessada = $"PRODUTO DIVERSO (CÓD {codProd})";
                                }

                                cupom.Itens.Add(new ItemCupom
                                {
                                    ProdutoCodigo = codProd,
                                    ProdutoDescricao = descProcessada,
                                    Unidade = unidPadr,
                                    Quantidade = Math.Round(Convert.ToDecimal(reader["Quantidade"]), 3),
                                    PrecoUnitario = Math.Round(Convert.ToDecimal(reader["PrecoUnitario"]), 2),
                                    ValorTotal = Math.Round(Convert.ToDecimal(reader["ValorTotal"]), 2)
                                });
                            }
                        }
                    }
                }
            }

            return listaCupons;
        }

        private string SanitizarTextoCompleto(string? texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return "PRODUTO DIVERSO";

            // 1. Limpa aspas, caracteres nulos (\ufffd) e símbolos estranhos
            string t = texto.Replace("\"", "").Replace("'", "").Replace("\ufffd", "").Trim();

            // 2. Converte vírgulas numéricas em ponto (ex: 1,5 LT -> 1.5 LT | 2,5 KG -> 2.5 KG | 0,25 -> 0.25)
            t = System.Text.RegularExpressions.Regex.Replace(t, @"(\d+),(\d+)", @"$1.$2");

            // 3. Normalização Unicode NFKD para remover TODOS os acentos (Ç, Í, É, Ã, Â, Ô, Ú, etc)
            string nfkd = t.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (char c in nfkd)
            {
                if (System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            // 4. Maiúsculas e higienização de múltiplos espaços em branco
            string limpo = System.Text.RegularExpressions.Regex.Replace(sb.ToString().ToUpper(), @"\s+", " ").Trim();
            return limpo.Trim('.', '"', '-', '_', ';', ',');
        }

        public List<AgendamentoOfertaSaaS> ObterOfertasPostgresDjango(string pgConnStr, DateTime dataRef)
        {
            var ofertas = new List<AgendamentoOfertaSaaS>();
            try
            {
                using (var conn = new Npgsql.NpgsqlConnection(pgConnStr))
                {
                    conn.Open();
                    string sql = @"
                        SELECT 
                            a.id,
                            a.loja_id,
                            COALESCE(l.nome, 'LOJA ' || a.loja_id) AS loja_nome,
                            a.produto_id,
                            COALESCE(p.nome, 'PRODUTO ' || a.produto_id) AS produto_nome,
                            '000' || a.produto_id AS codigo_balanca,
                            a.preco_oferta,
                            a.data_inicio,
                            a.data_fim,
                            a.ativo
                        FROM nucleo_agendamentooferta a
                        LEFT JOIN lojas l ON l.id = a.loja_id
                        LEFT JOIN produtos p ON p.id = a.produto_id
                        WHERE a.data_fim >= @DataRef
                        ORDER BY a.data_inicio DESC, a.id DESC;";

                    using (var cmd = new Npgsql.NpgsqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@DataRef", dataRef.Date);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var dIniObj = reader["data_inicio"];
                                var dFimObj = reader["data_fim"];
                                DateTime dtIni = dIniObj is DateOnly d1 ? d1.ToDateTime(TimeOnly.MinValue) : Convert.ToDateTime(dIniObj);
                                DateTime dtFim = dFimObj is DateOnly d2 ? d2.ToDateTime(TimeOnly.MinValue) : Convert.ToDateTime(dFimObj);

                                ofertas.Add(new AgendamentoOfertaSaaS
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    LojaId = Convert.ToInt32(reader["loja_id"]),
                                    LojaNome = SanitizarTextoCompleto(reader["loja_nome"].ToString()),
                                    ProdutoId = Convert.ToInt32(reader["produto_id"]),
                                    ProdutoNome = SanitizarTextoCompleto(reader["produto_nome"].ToString()),
                                    CodigoBalanca = reader["codigo_balanca"].ToString() ?? "000",
                                    PrecoOferta = Convert.ToDecimal(reader["preco_oferta"]),
                                    DataInicio = dtIni,
                                    DataFim = dtFim,
                                    Ativo = Convert.ToBoolean(reader["ativo"]),
                                    SincronizadoBalanca = true
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[AVISO POSTGRES] Falha ao consultar nucleo_agendamentooferta: {ex.Message}");
            }
            return ofertas;
        }
    }
}
