# 🥩 SaaS Empresarial Enterprise .NET 8

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16.0-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Firebird](https://img.shields.io/badge/Firebird-5.0-FF0000?style=for-the-badge&logo=firebird&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![Desenvolvido por SoftwareParedes](https://img.shields.io/badge/Desenvolvido%20por-SoftwareParedes-10b981?style=for-the-badge&logo=codefactor&logoColor=white)
![License](https://img.shields.io/badge/Licen%C3%A7a-Propriet%C3%A1ria-red?style=for-the-badge)

> **Eu desenvolvi o SaaS Empresarial Enterprise .NET 8 como a mais completa e lucrativa plataforma web multi-tenant para gestão de açougues, casas de carnes e redes de varejo alimentício do Brasil.**

---

## 👨‍💻 Sobre o Desenvolvedor & Autoridade do Projeto

Eu, **SoftwareParedes**, projetei e construí esta plataforma SaaS de nível **Enterprise** para eliminar de forma definitiva os 3 maiores gargalos financeiros dos donos de açougue:

1. **A Perda Invisível de Margem na Desossa de Carcaça (Res-Casada).**
2. **O Sangramento Financeiro em Taxas Não Auditadas de Cartões TEF/Maquininhas.**
3. **A Falta de Previsibilidade de Compra Preditiva de Gado/Carnes para o Fim de Semana.**

---

## 🎯 As 6 Grandes Funcionalidades Enterprise que Eu Construí

### 1. 🥩 Módulo de Desossa & Rendimento de Carcaça (Res-Casada)
Eu desenvolvi o motor de cálculo técnico de custo ponderado por kg do boi/suíno, considerando a substituição tributária (ST), peso líquido dos cortes nobres, descarte de sebo/pelanca e cálculo automático de margem de contribuição (Tabela vs. Oferta).

### 2. 💳 Fintech Embutida & Auditoria de Taxas TEF/Adquirentes
Eu criei o robô de auditoria financeira que cruza cada cupom emitido no PDV com o contrato das maquininhas (Stone, PagBank, Cielo, Rede, Ticket, Sodexo). O sistema detecta automaticamente divergências de alíquotas cobradas indevidamente e calcula o valor exato a ser recuperado em reais.

### 3. 🏢 Gerenciador Multi-Tenant & Assinaturas SaaS (Recurring MRR)
Eu estruturei a arquitetura multi-empresa com gestão de planos (Starter, Pro, Enterprise) e isolamento de dados por loja, permitindo que cada cliente administre suas unidades com autenticação JWT e edição de cadastro completa.

### 4. 🤖 Assistente Preditivo de Reposição de Estoque (AI Purchasing)
Eu implementei o algoritmo de inteligência de compras que analisa os últimos 14 dias de vendas no Firebird, aplica peso preditivo de fim de semana (Sexta, Sábado e Domingo) e calcula exatamente a **quantidade de Carcaças de Boi Casado** e quilos por corte nobre que o açougueiro precisa encomendar do frigorífico.

### 5. 📱 Hub WhatsApp Bot & Cardápio Digital em Tempo Real
Eu criei o gerador de cotação formatada com disparo direto para o WhatsApp dos clientes e um cardápio digital dinâmico com preços normais e promocionais (`% OFF`).

### 6. 📊 DRE Gerencial Completo & Curva ABC de Cortes (80/20)
Eu projetei o Demonstrativo de Resultado do Exercício (Receita Bruta ➔ Impostos/Taxas ➔ Receita Líquida ➔ CMV ➔ Margem Bruta ➔ OpEx ➔ EBITDA ➔ Lucro Líquido Real) e a Curva ABC 80/20 dos cortes de alto giro.

---

## 🏗️ Arquitetura de Engenharia de Software (Clean Architecture)

Eu projetei a solução utilizando o padrão de arquitetura em camadas em **C# .NET 8**:

```text
SaaS Empresarial Enterprise .NET 8
├── SaaS.Core         # Domínio, Entidades e Regras de Negócio Preditivas
├── SaaS.Data         # Conectores de Dados (Firebird ERP + PostgreSQL Central)
├── SaaS.Api          # Web API RESTful C# .NET 8 com Controllers e Swagger
├── SaaS.Tests        # Suíte de Testes Unitários Automatizados com xUnit
└── SaaS.Web          # Dashboard Responsivo de Alta Performance (Vanilla CSS/JS)
```

---

## 🧪 Suíte de Testes Automatizados & Integração Contínua (CI/CD)

Eu garanti que todo o código passe por testes unitários rigorosos em **xUnit** e pipeline automatizado no **GitHub Actions** antes de ir para produção:

```bash
# Execução da suíte de testes unitários
dotnet test HarrisonSaaS.sln
```

---

## 📋 Proprietário & Licença

**Desenvolvido por SoftwareParedes.** Todos os direitos reservados. Software SaaS proprietário de venda comercial restrita.
