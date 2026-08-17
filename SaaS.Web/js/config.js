// SaaS Central Configuration & API Environment
const API_CONFIG = {
    // URL Base da API do SaaS Empresarial.
    BASE_URL: window.location.hostname === 'localhost' || window.location.hostname === '127.0.0.1' 
        ? 'http://localhost:5064/api' 
        : 'https://ties-villas-partially-trans.trycloudflare.com/api',
        
    ENDPOINTS: {
        LOGIN: '/ModulosSaaS/auth/login',
        DASHBOARD: '/ModulosSaaS/dashboard',
        TV_OFERTAS: '/ModulosSaaS/tv-ofertas',
        TEF_STATUS: '/ModulosSaaS/tef-status'
    }
};

/**
 * Função utilitária para higienização de strings contra ataques XSS
 */
function sanitizeHTML(str) {
    if (!str) return '';
    return String(str)
        .replace(/&/g, '&amp;')
        .replace(/</g, '&lt;')
        .replace(/>/g, '&gt;')
        .replace(/"/g, '&quot;')
        .replace(/'/g, '&#039;');
}

/**
 * Utilitários de Gerenciamento de Autenticação Segura (JWT)
 */
const AuthService = {
    getToken() {
        return localStorage.getItem('saas_jwt_token');
    },
    getUsuario() {
        return localStorage.getItem('saas_usuario_logado');
    },
    getTenantId() {
        return localStorage.getItem('saas_tenant_id');
    },
    setSession(token, usuario, tenantId) {
        localStorage.setItem('saas_jwt_token', token);
        localStorage.setItem('saas_usuario_logado', usuario);
        localStorage.setItem('saas_tenant_id', tenantId);
    },
    clearSession() {
        localStorage.removeItem('saas_jwt_token');
        localStorage.removeItem('saas_usuario_logado');
        localStorage.removeItem('saas_tenant_id');
    },
    isAuthenticated() {
        const token = this.getToken();
        if (!token || token.trim() === '') return false;
        
        try {
            const parts = token.split('.');
            if (parts.length === 3) {
                const payloadBase64 = parts[1];
                const decodedJson = atob(payloadBase64.replace(/-/g, '+').replace(/_/g, '/'));
                const decoded = JSON.parse(decodedJson);
                
                if (decoded.exp && decoded.exp * 1000 < Date.now()) {
                    this.clearSession();
                    return false;
                }
            }
            return true;
        } catch (e) {
            // Em caso de formato customizado de token ou erro de atob, se existe token assume válido
            return true;
        }
    },
    checkAuthOrRedirect() {
        if (!this.isAuthenticated()) {
            this.clearSession();
            window.location.href = 'login.html';
        }
    }
};

/**
 * Wrapper seguro para chamadas HTTP Fetch com JWT Header automático e tratamento de 401
 */
async function fetchWithAuth(endpoint, options = {}) {
    const token = AuthService.getToken();
    const headers = {
        'Content-Type': 'application/json',
        ...(options.headers || {})
    };

    if (token) {
        headers['Authorization'] = `Bearer ${token}`;
    }

    const url = endpoint.startsWith('http') ? endpoint : `${API_CONFIG.BASE_URL}${endpoint}`;

    try {
        const response = await fetch(url, { ...options, headers });
        
        if (response.status === 401) {
            AuthService.clearSession();
            if (!window.location.pathname.endsWith('login.html')) {
                window.location.href = 'login.html';
            }
            throw new Error('Sessão expirada. Faça login novamente.');
        }
        
        return response;
    } catch (error) {
        console.error('Erro na requisição API:', error);
        throw error;
    }
}
