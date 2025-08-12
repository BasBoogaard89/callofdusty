import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { RoomCategoryDto, RoomDto } from '@generated/api';
import { RoomAdapterService } from '@infrastructure/adapters/room-adapter.service';
import { RoomCategoryAdapterService } from '@infrastructure/adapters/room-category-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';
import { RoomCategoryForm } from '../../room-category/room-category-form/room-category-form';

@Component({
    selector: 'app-room-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './room-form.html'
})
export class RoomForm extends BaseFormController<RoomDto> {
    service = inject(RoomAdapterService);
    categoryService = inject(RoomCategoryAdapterService);
    listRoutePrefix = 'room';

    form = new FormGroup({
        id: new FormControl(0),
        description: new FormControl('', Validators.required),
        categoryId: new FormControl(null, Validators.required),
    });
    
    categories = signal<RoomCategoryDto[]>([]);

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'description', type: 'text', label: 'Description' },
            { key: 'categoryId', type: 'select', label: 'Category',
                options: () => this.categories().map(e => ({ value: e.id, label: e.description })),
                create: async () => this.openCreateDialog(RoomCategoryForm)
            },
        ]
    }));

    protected override async loadAdditionalData() {
        this.categories.set(await this.categoryService.getAll());
    }
}
