import { computed, inject, Injectable, signal } from '@angular/core';
import { ThemeDto } from '@generated/api';
import { ThemeAdapterService } from '@infrastructure/adapters/theme-adapter.service';

@Injectable({
    providedIn: 'root'
})
export class ThemeSelectionService {
    themeService = inject(ThemeAdapterService);

    readonly themes = signal<ThemeDto[]>([]);
    private readonly selectedId = signal<number | null>(null);

    readonly selected = computed<ThemeDto | null>(() => {
        const id = this.selectedId();
        return id == null ? null : this.themes().find(t => t.id === id) ?? null;
    });

    async loadThemes(): Promise<void> {
        const dtos = await this.themeService.getAll();
        this.themes.set(dtos);
        if (this.selectedId() == null && dtos.length) {
            this.selectedId.set(dtos[0].id);
        }
    }

    selectTheme(theme: ThemeDto | number) {
        const id = typeof theme === 'number' ? theme : theme.id;
        if (this.selectedId() !== id) this.selectedId.set(id);
    }
}
