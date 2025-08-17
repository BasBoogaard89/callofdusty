import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthAdapterService } from '@infrastructure/adapters/auth-adapter.service';

export const guestGuard: CanActivateFn = (route, state) => {
    const auth = inject(AuthAdapterService);
    const router = inject(Router);

    return !auth.isAuthenticated() ? true : router.parseUrl('/');
};
