import { Component, effect, inject } from '@angular/core';
import { Router } from '@angular/router';
import { AuthAdapterService } from '@infrastructure/adapters/auth-adapter.service';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';
import { ThemeSelectionService } from '@infrastructure/services/theme-selection.service';

@Component({
    selector: 'app-header',
    standalone: true,
    imports: [...MATERIAL_IMPORTS],
    templateUrl: './header.html'
})
export class Header {
    themeSelectionService = inject(ThemeSelectionService);
    authService = inject(AuthAdapterService);
    router = inject(Router);

    themes = this.themeSelectionService.themes;
    selected = this.themeSelectionService.selected;

    constructor() {
        effect(() => {
            if (this.authService.isAuthenticated()) {
                this.loadThemes();
            }
        })
    }

    loadThemes() {
        this.themeSelectionService.loadThemes();
    }

    onThemeChange(id: number) {
        this.themeSelectionService.selectTheme(id);
    }

    logout() {
        this.authService.logout()
            .then(() => {
                this.router.navigateByUrl('/login');
            });
    }
}
