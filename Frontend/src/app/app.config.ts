import { provideAnimations } from "@angular/platform-browser/animations";
import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { AuthAdapterService } from "@infrastructure/adapters/auth-adapter.service";

export const appConfig: ApplicationConfig = {
    providers: [
        provideAnimations(),
        provideBrowserGlobalErrorListeners(),
        provideZoneChangeDetection({ eventCoalescing: true }),
        provideRouter(routes),
        provideAppInitializer(() => {
            const auth = inject(AuthAdapterService);
            return auth.refreshUser();
        }),
    ]
};
