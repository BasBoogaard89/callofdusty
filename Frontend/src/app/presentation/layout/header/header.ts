import { Component, inject } from '@angular/core';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';
import { ThemeSelectionService } from '@infrastructure/services/theme-selection.service';

@Component({
    selector: 'app-header',
    imports: [...MATERIAL_IMPORTS],
    templateUrl: './header.html'
})
export class Header {
    themeSelectionService = inject(ThemeSelectionService);

    themes = this.themeSelectionService.themes;
    selected = this.themeSelectionService.selected; 

    ngOnInit() {
        this.themeSelectionService.loadThemes();
    }

    onThemeChange(id: number) {
        this.themeSelectionService.selectTheme(id);
    }
}
