// ============================================================================
// 📊 MÓDULO ISOLADO: ENTRADA DE VALORES & AUDITORIA DE CAIXA (POSTGRESQL)
// Trabalha exclusivamente com a tabela public.vendas_diarias do PostgreSQL.
// Renderiza os 14 cards em layout 7x2 rigoroso.
// Edição inline entre o painel e a tabela sem rolagem para o fim da página.
// ============================================================================

(function () {
    let filtroAnoEV = 2026;
    let filtroMesEV = new Date().getMonth() + 1; // 1-12
    let filtroDiaEV = null; // null = Mês Inteiro
    window.vendasDiariasCache = [];

    window.EV_carregarVendasDiariasEntrada = async function () {
        try {
            const lblAno = document.getElementById('ev-lbl-filtro-ano');
            if (lblAno) lblAno.innerText = filtroAnoEV;

            const res = await fetchWithAuth('/ModulosSaaS/vendas-diarias?ano=' + filtroAnoEV + '&mes=' + filtroMesEV);
            const data = await res.json();
            window.vendasDiariasCache = data || [];

            EV_renderizarNavegadorData();
            EV_filtrarVendasDiariasLocal();
        } catch (err) {
            console.error('[ERRO MÓDULO ENTRADA DE VALORES] Falha ao carregar PostgreSQL:', err);
        }
    };

    window.EV_mudarAnoFiltro = function (delta) {
        filtroAnoEV += delta;
        const lblAno = document.getElementById('ev-lbl-filtro-ano');
        if (lblAno) lblAno.innerText = filtroAnoEV;
        window.EV_carregarVendasDiariasEntrada();
    };

    window.EV_selecionarMesFiltro = function (mesNum) {
        filtroMesEV = mesNum;
        filtroDiaEV = null;
        EV_renderizarNavegadorData();
        window.EV_carregarVendasDiariasEntrada();
    };

    window.EV_selecionarDiaFiltro = function (diaNum) {
        filtroDiaEV = diaNum;
        EV_renderizarNavegadorData();
        EV_filtrarVendasDiariasLocal();
    };

    function EV_renderizarNavegadorData() {
        const containerMeses = document.getElementById('ev-nav-meses-container');
        const containerDias = document.getElementById('ev-nav-dias-container');
        if (!containerMeses || !containerDias) return;

        const nomesMesesPt = ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'];

        let htmlMeses = '';
        nomesMesesPt.forEach((mNome, idx) => {
            const mesNum = idx + 1;
            const activeStyle = mesNum === filtroMesEV
                ? 'background: var(--primary); color: #fff; border-color: var(--primary); font-weight: 800;'
                : 'background: var(--bg-card); color: var(--text-muted); border-color: var(--border-color);';
            htmlMeses += '<button type="button" class="btn-action" style="padding: 0.3rem 0.65rem; font-size: 0.78rem; border-radius: 6px; border: 1px solid; ' + activeStyle + '" onclick="window.EV_selecionarMesFiltro(' + mesNum + ')">' + mNome.substring(0, 3) + '</button>';
        });
        containerMeses.innerHTML = htmlMeses;

        const qtdDiasNoMes = new Date(filtroAnoEV, filtroMesEV, 0).getDate();
        const nomeMesSel = nomesMesesPt[filtroMesEV - 1];

        let htmlDias = '<span style="font-size: 0.78rem; font-weight: 700; color: var(--text-muted); margin-right: 0.25rem;">DIAS DE ' + nomeMesSel.toUpperCase() + ':</span>';

        const activeMesInteiro = filtroDiaEV === null
            ? 'background: var(--accent-cyan); color: #000; font-weight: 800;'
            : 'background: var(--bg-card); color: var(--text-muted);';
        htmlDias += '<button type="button" class="btn-action" style="padding: 0.25rem 0.55rem; font-size: 0.75rem; border-radius: 6px; ' + activeMesInteiro + '" onclick="window.EV_selecionarDiaFiltro(null)">Todos (' + nomeMesSel + ')</button>';

        for (let d = 1; d <= qtdDiasNoMes; d++) {
            const activeDia = filtroDiaEV === d
                ? 'background: var(--accent-cyan); color: #000; font-weight: 800;'
                : 'background: var(--bg-card); color: var(--text-muted);';
            htmlDias += '<button type="button" class="btn-action" style="padding: 0.25rem 0.5rem; font-size: 0.75rem; border-radius: 6px; ' + activeDia + '" onclick="window.EV_selecionarDiaFiltro(' + d + ')">' + d + ' de ' + nomeMesSel + '</button>';
        }
        containerDias.innerHTML = htmlDias;
    }

    function EV_filtrarVendasDiariasLocal() {
        const todasVendas = window.vendasDiariasCache || [];
        const lojaSel = parseInt(document.getElementById('ev-filtro-loja') ? document.getElementById('ev-filtro-loja').value : 0);

        const filtradas = todasVendas.filter(v => {
            if (!v.data) return false;
            const dStr = v.data.split('T')[0];
            const [ano, mes, dia] = dStr.split('-').map(Number);

            if (lojaSel > 0 && v.lojaId !== lojaSel) return false;
            if (ano !== filtroAnoEV) return false;
            if (mes !== filtroMesEV) return false;
            if (filtroDiaEV !== null && dia !== filtroDiaEV) return false;
            return true;
        });

        const countLbl = document.getElementById('ev-dias-count');
        if (countLbl) countLbl.innerText = '(' + filtradas.length + ' dias exibidos)';

        // 14 SOMAS NA ORDEM EXATA SOLICITADA (7 EM CIMA E 7 EM BAIXO):
        let sDinheiro = 0;
        let sTrocoAmanha = 0;
        let sCartaoVendido = 0;
        let sCartaoRecebido = 0;
        let sPixVendido = 0;
        let sConvenioPago = 0;
        let sTarifaPix = 0;
        let sConvenioVenda = 0;
        let sTaxaEntrega = 0;
        let sTarifaCartao = 0;
        let sSangria = 0;
        let sDesconto = 0;
        let sTotalGaveta = 0;
        let sQuebraCaixa = 0;

        filtradas.forEach(v => {
            sDinheiro += (v.dinheiro || 0);
            sTrocoAmanha += (v.trocoParaAmanha || 0);
            sCartaoVendido += (v.cartaoVendido || 0);
            sCartaoRecebido += (v.cartaoRecebido || 0);
            sPixVendido += (v.pixVendido || 0);
            sConvenioPago += (v.convenioPago || 0);
            sTarifaPix += (v.tarifaPix || 0);
            sConvenioVenda += (v.convenioVenda || 0);
            sTaxaEntrega += (v.taxaEntrega || 0);
            sTarifaCartao += (v.tarifaCartao || 0);
            sSangria += (v.sangria || 0);
            sDesconto += (v.desconto || 0);
            sTotalGaveta += (v.totalGaveta || 0);
            sQuebraCaixa += (v.quebraCaixa || 0);
        });

        const fmt = (val) => 'R$ ' + val.toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

        const setTxt = (id, txt) => {
            const el = document.getElementById(id);
            if (el) el.innerText = txt;
        };

        // Atualização dos 14 cards em layout 7x2
        setTxt('ev-card-dinheiro', fmt(sDinheiro));
        setTxt('ev-card-troco-amanha', fmt(sTrocoAmanha));
        setTxt('ev-card-cartao-vendido', fmt(sCartaoVendido));
        setTxt('ev-card-cartao-recebido', fmt(sCartaoRecebido));
        setTxt('ev-card-pix-vendido', fmt(sPixVendido));
        setTxt('ev-card-convenio-pago', fmt(sConvenioPago));
        setTxt('ev-card-tarifa-pix', fmt(sTarifaPix));
        setTxt('ev-card-convenio-venda', fmt(sConvenioVenda));
        setTxt('ev-card-taxa-entrega', fmt(sTaxaEntrega));
        setTxt('ev-card-tarifa-cartao', fmt(sTarifaCartao));
        setTxt('ev-card-sangria', fmt(sSangria));
        setTxt('ev-card-desconto', fmt(sDesconto));
        setTxt('ev-card-total-gaveta', fmt(sTotalGaveta));
        const cardQuebraEl = document.getElementById('ev-card-quebra-caixa');
        if (cardQuebraEl) {
            cardQuebraEl.innerText = fmt(sQuebraCaixa);
            const parentCard = cardQuebraEl.closest('.kpi-card');
            if (sQuebraCaixa >= 0) {
                cardQuebraEl.style.color = 'var(--accent-green)';
                if (parentCard) parentCard.style.borderColor = 'rgba(16, 185, 129, 0.4)';
            } else {
                cardQuebraEl.style.color = 'var(--accent-rose)';
                if (parentCard) parentCard.style.borderColor = 'rgba(244, 63, 94, 0.4)';
            }
        }

        // Preencher Tabela (Sem a coluna Ações; Data é o link direto para editar)
        const tbody = document.getElementById('ev-table-body');
        if (!tbody) return;
        tbody.innerHTML = '';

        if (filtradas.length === 0) {
            tbody.innerHTML = '<tr><td colspan="16" style="text-align: center; color: var(--accent-amber); padding: 2rem; font-weight: 700;">Nenhum registro em public.vendas_diarias encontrado para o filtro.</td></tr>';
            return;
        }

        filtradas.forEach(v => {
            const dataFmt = v.data ? v.data.split('T')[0] : '-';
            const tr = document.createElement('tr');
            tr.innerHTML = '<td style="font-weight: 800;">' +
                '<a onclick="window.EV_editarVendaDiariaModal(' + v.id + ')" style="color: var(--accent-cyan); text-decoration: underline; cursor: pointer; display: inline-flex; align-items: center; gap: 0.35rem;" title="Clique para abrir a edição deste dia no topo da tela">' +
                '<i class="fa-solid fa-pen-to-square" style="font-size: 0.85rem;"></i> ' + dataFmt + '</a></td>' +
                '<td style="font-weight: 700;">' + sanitizeHTML(v.lojaNome) + '</td>' +
                '<td style="color: var(--accent-green); font-weight: 700;">' + fmt(v.dinheiro || 0) + '</td>' +
                '<td>' + fmt(v.trocoParaAmanha || 0) + '</td>' +
                '<td style="color: var(--accent-amber); font-weight: 700;">' + fmt(v.cartaoVendido || 0) + '</td>' +
                '<td style="color: var(--accent-amber);">' + fmt(v.cartaoRecebido || 0) + '</td>' +
                '<td style="color: var(--accent-cyan); font-weight: 700;">' + fmt(v.pixVendido || 0) + '</td>' +
                '<td>' + fmt(v.convenioPago || 0) + '</td>' +
                '<td>' + fmt(v.tarifaPix || 0) + '</td>' +
                '<td>' + fmt(v.convenioVenda || 0) + '</td>' +
                '<td>' + fmt(v.taxaEntrega || 0) + '</td>' +
                '<td>' + fmt(v.tarifaCartao || 0) + '</td>' +
                '<td style="color: var(--accent-rose); font-weight: 700;">' + fmt(v.sangria || 0) + '</td>' +
                '<td>' + fmt(v.desconto || 0) + '</td>' +
                '<td style="font-weight: 900; color: #fff;">' + fmt(v.totalGaveta || 0) + '</td>' +
                '<td>' + fmt(v.quebraCaixa || 0) + '</td>';
            tbody.appendChild(tr);
        });
    }

    window.EV_abrirModalNovaVendaDiaria = function () {
        document.getElementById('ev-modal-title').innerHTML = '<i class="fa-solid fa-plus-circle" style="color: var(--accent-green);"></i> Nova Entrada de Valores em public.vendas_diarias';
        document.getElementById('ev-id').value = '0';
        document.getElementById('ev-loja').value = '1';
        document.getElementById('ev-data').value = new Date().toISOString().split('T')[0];
        document.getElementById('ev-dinheiro').value = '0,00';
        document.getElementById('ev-troco-amanha').value = '0,00';
        document.getElementById('ev-cartao-vendido').value = '0,00';
        document.getElementById('ev-cartao-recebido').value = '0,00';
        document.getElementById('ev-pix').value = '0,00';
        document.getElementById('ev-convenio-pago').value = '0,00';
        document.getElementById('ev-tarifa-pix').value = '0,00';
        document.getElementById('ev-convenio-venda').value = '0,00';
        document.getElementById('ev-taxa-entrega').value = '0,00';
        document.getElementById('ev-tarifa-cartao').value = '0,00';
        document.getElementById('ev-sangria').value = '0,00';
        document.getElementById('ev-desconto').value = '0,00';
        document.getElementById('ev-total-gaveta').value = '0,00';
        document.getElementById('ev-quebra-caixa').value = '0,00';

        const pInline = document.getElementById('ev-panel-inline-edit');
        if (pInline) {
            pInline.style.display = 'block';
            pInline.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    };

    window.EV_editarVendaDiariaModal = function (id) {
        const lista = window.vendasDiariasCache || [];
        const item = lista.find(v => v.id === id);
        if (!item) return;

        const fmtVal = (val) => (val || 0).toLocaleString('pt-BR', { minimumFractionDigits: 2, maximumFractionDigits: 2 });

        document.getElementById('ev-modal-title').innerHTML = '<i class="fa-solid fa-edit" style="color: var(--accent-green);"></i> Editar Registro #' + item.id + ' — Data: ' + (item.data ? item.data.split('T')[0] : '-') + ' (' + item.lojaNome + ')';
        document.getElementById('ev-id').value = item.id;
        document.getElementById('ev-loja').value = item.lojaId;
        document.getElementById('ev-data').value = item.data ? item.data.split('T')[0] : new Date().toISOString().split('T')[0];
        document.getElementById('ev-dinheiro').value = fmtVal(item.dinheiro);
        document.getElementById('ev-troco-amanha').value = fmtVal(item.trocoParaAmanha);
        document.getElementById('ev-cartao-vendido').value = fmtVal(item.cartaoVendido);
        document.getElementById('ev-cartao-recebido').value = fmtVal(item.cartaoRecebido);
        document.getElementById('ev-pix').value = fmtVal(item.pixVendido);
        document.getElementById('ev-convenio-pago').value = fmtVal(item.convenioPago);
        document.getElementById('ev-tarifa-pix').value = fmtVal(item.tarifaPix);
        document.getElementById('ev-convenio-venda').value = fmtVal(item.convenioVenda);
        document.getElementById('ev-taxa-entrega').value = fmtVal(item.taxaEntrega);
        document.getElementById('ev-tarifa-cartao').value = fmtVal(item.tarifaCartao);
        document.getElementById('ev-sangria').value = fmtVal(item.sangria);
        document.getElementById('ev-desconto').value = fmtVal(item.desconto);
        document.getElementById('ev-total-gaveta').value = fmtVal(item.totalGaveta);
        document.getElementById('ev-quebra-caixa').value = fmtVal(item.quebraCaixa);

        const pInline = document.getElementById('ev-panel-inline-edit');
        if (pInline) {
            pInline.style.display = 'block';
            pInline.scrollIntoView({ behavior: 'smooth', block: 'center' });
        }
    };

    window.EV_fecharModalVendaDiaria = function () {
        const pInline = document.getElementById('ev-panel-inline-edit');
        if (pInline) pInline.style.display = 'none';
    };

    window.EV_salvarVendaDiariaModal = async function (e) {
        e.preventDefault();
        const parseVal = (id) => parseFloat(document.getElementById(id).value.replace(/\./g, '').replace(',', '.')) || 0;

        const dto = {
            id: parseInt(document.getElementById('ev-id').value) || 0,
            lojaId: parseInt(document.getElementById('ev-loja').value) || 1,
            data: document.getElementById('ev-data').value,
            dinheiro: parseVal('ev-dinheiro'),
            trocoParaAmanha: parseVal('ev-troco-amanha'),
            cartaoVendido: parseVal('ev-cartao-vendido'),
            cartaoRecebido: parseVal('ev-cartao-recebido'),
            pixVendido: parseVal('ev-pix'),
            convenioPago: parseVal('ev-convenio-pago'),
            tarifaPix: parseVal('ev-tarifa-pix'),
            convenioVenda: parseVal('ev-convenio-venda'),
            taxaEntrega: parseVal('ev-taxa-entrega'),
            tarifaCartao: parseVal('ev-tarifa-cartao'),
            sangria: parseVal('ev-sangria'),
            desconto: parseVal('ev-desconto'),
            totalGaveta: parseVal('ev-total-gaveta'),
            quebraCaixa: parseVal('ev-quebra-caixa')
        };

        try {
            const res = await fetchWithAuth('/ModulosSaaS/vendas-diarias', {
                method: 'POST',
                body: JSON.stringify(dto)
            });
            if (res.ok) {
                alert('✅ Registro salvo com sucesso na tabela public.vendas_diarias!');
                window.EV_fecharModalVendaDiaria();
                window.EV_carregarVendasDiariasEntrada();
            } else {
                alert('⚠️ Erro ao salvar registro no PostgreSQL!');
            }
        } catch (err) {
            alert('⚠️ Falha na comunicação com a API do SaaS!');
        }
    };
})();
