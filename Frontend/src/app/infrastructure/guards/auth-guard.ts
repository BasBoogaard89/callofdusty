import { inject } from '@angular/core';
import { CanActivateChildFn, CanActivateFn, Router } from '@angular/router';
import { AuthAdapterService } from '@infrastructure/adapters/auth-adapter.service';

export const authGuard: CanActivateFn = (route, state) => {
    const auth = inject(AuthAdapterService);
    const router = inject(Router);

    return auth.isAuthenticated() ? true : router.parseUrl('/login');
};

export const authGuardChild: CanActivateChildFn = (route, state) => {
    const auth = inject(AuthAdapterService);
    const router = inject(Router);
    
    return auth.isAuthenticated() ? true : router.parseUrl('/login');
};
