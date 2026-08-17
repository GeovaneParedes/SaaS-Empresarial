# 🥩 HarrisonSaaS — Plataforma SaaS Empresarial para Açougues

![.NET 8](https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![PostgreSQL](https://img.shields.io/badge/PostgreSQL-16.0-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)
![Firebird](https://img.shields.io/badge/Firebird-5.0-FF0000?style=for-the-badge&logo=firebird&logoColor=white)
![Build & CI/CD Status](https://img.shields.io/badge/CI%2FCD-GitHub%20Actions-2088FF?style=for-the-badge&logo=githubactions&logoColor=white)
![License](https://img.shields.io/badge/License-Proprietary-red?style=for-the-badge)

> **Plataforma SaaS de Gestão de Açougues, Módulo de Desossa & Rendimento de Carcaça (Res-Casada), Conciliação de Caixa em Tempo Real e Agendamento Inteligente de Ofertas.**

---

## 🏛️ Visão Geral do Sistema

O **HarrisonSaaS** é um software de gestão de nível **Enterprise** projetado para resolver os maiores gargalos operacionais e financeiros de redes de açougues e casas de carnes:

1. **🥩 Módulo de Desossa & Rendimento de Carcaça (Res-Casada):** Cálculo técnico de custo ponderado por kg, substituição tributaria, peso líquido, margem de oferta vs. tabela e projeção de lucro líquido por boi/suíno.
2. **💳 Conciliação de Caixa & Maquininhas TEF:** Cálculo automático de tarifas de cartão de crédito/débito, PIX e sangria de gaveta em tempo real.
3. **🏷️ Agendamento Inteligente de Ofertas:** Sincronização direta com bancos de dados PostgreSQL/Django e atualização automática de balanças eletrônicas Toledo (MGV6/MGV7).
4. **📺 Painéis Digitais de TV (Digital Signage):** Telas de exibição de ofertas em tempo real para TV nas lojas por unidade.

---

## 🏗️ Arquitetura Multi-Tenant

A solução é desenvolvida em **C# .NET 8** utilizando o padrão **Clean Architecture + Multi-Tenant Hybrid Isolation**:

- **`HarrisonSaaS.Core`:** Domínio, entidades e regras de negócio de precificação.
- **`HarrisonSaaS.Data`:** Serviços de sincronização Firebird ERP e banco central PostgreSQL.
- **`HarrisonSaaS.Api`:** Controllers RESTful documentados via Swagger OpenAPI.
- **`HarrisonSaaS.Tests`:** Suíte de testes unitários automatizados em `xUnit`.
- **`HarrisonSaaS.Web`:** Dashboard responsivo de alta performance e visualização de ofertas.

---

## 🧪 Suíte de Testes & Qualidade de Código

Para garantir 100% de estabilidade financeira e operacional em produção:

```bash
# Execução da suíte de testes unitários xUnit
dotnet test HarrisonSaaS.sln
```

---

## 📋 Licença & Direitos

Este projeto é um software proprietário SaaS de venda comercial. Todos os direitos reservados.
