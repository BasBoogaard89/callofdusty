import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { ChoreDto, HistoryDto } from '@generated/api';
import { ChoreAdapterService } from '@infrastructure/adapters/chore-adapter.service';
import { HistoryAdapterService } from '@infrastructure/adapters/history-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';

@Component({
    selector: 'app-history-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './history-form.html'
})
export class HistoryForm extends BaseFormController<HistoryDto> {
    service = inject(HistoryAdapterService);
    choreService = inject(ChoreAdapterService);
    listRoutePrefix = 'history';

    form = new FormGroup({
        id: new FormControl(0),
        choreId: new FormControl(null, Validators.required),
        dateStarted: new FormControl(null, Validators.required),
        dateCompleted: new FormControl(null, Validators.required),
    });

    chores = signal<ChoreDto[]>([]);

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'description', type: 'text', label: 'Description' },
            { key: 'choreId', type: 'select', label: 'Chore', options: () => this.chores().map(e => ({ value: e.id, label: e.description })) },
            { key: 'dateStarted', type: 'date', label: 'Date started' },
            { key: 'dateCompleted', type: 'date', label: 'Date completed' }
        ]
    }));

    protected override async loadAdditionalData() {
        this.chores.set(await this.choreService.getAll());
    }
}
