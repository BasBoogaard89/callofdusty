import { CommonModule, JsonPipe } from '@angular/common';
import { Component, effect, inject, signal } from '@angular/core';
import { ChoreQuestDto } from '@generated/api';
import { ChoreAdapterService } from '@infrastructure/adapters/chore-adapter.service';
import { BasePageController } from '@infrastructure/controllers/base-page.controller';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';
import { ThemeSelectionService } from '@infrastructure/services/theme-selection.service';

@Component({
    selector: 'app-quest',
    standalone: true,
    imports: [CommonModule, JsonPipe, ...MATERIAL_IMPORTS],
    templateUrl: './quest.html',
})
export class Quest extends BasePageController {
    choreService: ChoreAdapterService = inject(ChoreAdapterService);
    themeSelectionService: ThemeSelectionService = inject(ThemeSelectionService);

    quests: ChoreQuestDto[] = [];
    flipped = signal(new Set<number>());

    constructor() {
        super();
        effect(async () => {
            const theme = this.themeSelectionService.selected();
            if (!theme) return;
            
            this.quests = await this.choreService.getAllQuests(theme.id);
        });
    }

    async ngOnInit() {
        this.setDynamicTitle();

        await this.themeSelectionService.loadThemes();
    }
    
    isOpen(id: number) {
        return this.flipped().has(id);
    }

    toggle(id: number) {
        const s = new Set(this.flipped());
        s.has(id) ? s.delete(id) : s.add(id);
        this.flipped.set(s);
    }

    startQuest(quest: ChoreQuestDto) {
        console.log("Starting new quest", quest);
    }
}
