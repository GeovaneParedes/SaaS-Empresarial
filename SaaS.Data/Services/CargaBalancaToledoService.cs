using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SaaS.Core.Entities;

namespace SaaS.Data.Services
{
    public class CargaBalancaToledoService
    {
        /// <summary>
        /// Gera o arquivo CADTXT.TXT no padrão oficial Toledo MGV (Mgv6/Mgv7)
        /// Estrutura por item PLU: 
        ///  - Tipo: 01 (Peso/Kg)
        ///  - Código PLU: 6 dígitos
        ///  - Preço por Kg: 6 dígitos (sem vírgula)
        ///  - Validade em dias: 3 dígitos
        ///  - Descrição do corte de carne: 50 caracteres
        /// </summary>
        public string GerarArquivoCargaToledo(List<Produto> produtos, string diretorioDestino = "/tmp/TOLEDO")
        {
            if (!Directory.Exists(diretorioDestino))
            {
                Directory.CreateDirectory(diretorioDestino);
            }

            string caminhoArquivo = Path.Combine(diretorioDestino, "CADTXT.TXT");
            var sb = new StringBuilder();

            foreach (var p in produtos)
            {
                // Formatação estrita da linha da Toledo MGV:
                // Exemplo: 010001230004499003ALCATRA MATURADA
                string pluStr = p.CodigoBalanca.PadLeft(6, '0');
                string precoStr = ((int)(p.PrecoTabela * 100)).ToString().PadLeft(6, '0');
                string validadeStr = p.ValidadeDias.ToString().PadLeft(3, '0');
                string descStr = p.Nome.Length > 50 ? p.Nome.Substring(0, 50) : p.Nome.PadRight(50, ' ');

                string linha = $"01{pluStr}{precoStr}{validadeStr}{descStr}";
                sb.AppendLine(linha);
            }

            File.WriteAllText(caminhoArquivo, sb.ToString(), Encoding.GetEncoding("iso-8859-1"));
            return caminhoArquivo;
        }
    }
}
