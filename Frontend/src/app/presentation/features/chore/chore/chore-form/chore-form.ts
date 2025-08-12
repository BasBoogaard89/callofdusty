import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { ChoreCategoryDto, ChoreDto, DirtinessFactor, RoomDto } from '@generated/api';
import { ChoreAdapterService } from '@infrastructure/adapters/chore-adapter.service';
import { ChoreCategoryAdapterService } from '@infrastructure/adapters/chore-category-adapter.service';
import { RoomAdapterService } from '@infrastructure/adapters/room-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';
import { ChoreCategoryForm } from '../../chore-category/chore-category-form/chore-category-form';
import { RoomForm } from '@presentation/features/room/room/room-form/room-form';

@Component({
    selector: 'app-chore-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './chore-form.html'
})
export class ChoreForm extends BaseFormController<ChoreDto> {
    service = inject(ChoreAdapterService);
    roomService = inject(RoomAdapterService);
    categoryService = inject(ChoreCategoryAdapterService);

    listRoutePrefix = 'chore';

    form = new FormGroup({
        id: new FormControl(0),
        description: new FormControl('', Validators.required),
        categoryId: new FormControl(null, Validators.required),
        roomId: new FormControl(null, Validators.required),
        durationMinutes: new FormControl(5, Validators.required),
        frequencyDays: new FormControl(7, Validators.required),
        dirtinessFactor: new FormControl(0, Validators.required)
    });

    rooms = signal<RoomDto[]>([]);
    categories = signal<ChoreCategoryDto[]>([]);
    dirtinessOptions = Object.values(DirtinessFactor);

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'description', type: 'text', label: 'Description' },
            { key: 'categoryId', type: 'select', label: 'Category',
                options: () => this.categories().map(e => ({ value: e.id, label: e.description })),
                create: async () => this.openCreateDialog(ChoreCategoryForm),
            },
            { key: 'roomId', type: 'select', label: 'Room',
                options: () => this.rooms().map(e => ({ value: e.id, label: e.description })),
                create: async () => this.openCreateDialog(RoomForm), },
            { key: 'durationMinutes', type: 'number', label: 'Duration in minutes' },
            { key: 'frequencyDays', type: 'number', label: 'Frequency in days' },
            { key: 'dirtinessFactor', type: 'select', label: 'Dirtiness factor', options: () => this.dirtinessOptions.map(e => ({ value: e, label: e })) },
        ]
    }));

    protected override async loadAdditionalData() {
        this.rooms.set(await this.roomService.getAll());
        this.categories.set(await this.categoryService.getAll());
    }
}
