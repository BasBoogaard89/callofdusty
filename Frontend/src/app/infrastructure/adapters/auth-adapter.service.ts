import { computed, Injectable, signal } from '@angular/core';
import { AuthService, LoginRequest, OpenAPI } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

export interface CurrentUser {
    id: string;
    email: string;
    roles: string[];
}

@Injectable({
    providedIn: 'root'
})
export class AuthAdapterService {
    api = new AuthService(new AxiosHttpRequest({
        ...OpenAPI,
        WITH_CREDENTIALS: true,
    }));

    private userSig = signal<CurrentUser | null>(null);
    isAuthenticated = computed(() => this.userSig() !== null);

    get user() {
        return this.userSig.asReadonly();
    }

    async login(request: LoginRequest) {
        await this.api.postAuthLogin(request);
        await this.refreshUser();
    }

    async logout() {
        await this.api.postAuthLogout();
        this.userSig.set(null);
    }

    async refreshUser() {
        try {
            const me = await this.api.getAuthMe();
            this.userSig.set(me as CurrentUser);
        } catch {
            this.userSig.set(null);
        }
    }
}
