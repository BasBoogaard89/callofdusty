import { Component, computed, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { RoomCategoryDto } from '@generated/api';
import { RoomCategoryAdapterService } from '@infrastructure/adapters/room-category-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';

@Component({
    selector: 'app-room-category-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './room-category-form.html'
})
export class RoomCategoryForm extends BaseFormController<RoomCategoryDto> {
    service = inject(RoomCategoryAdapterService);
    listRoutePrefix = 'room/category';

    form = new FormGroup({
        id: new FormControl(0),
        description: new FormControl('', Validators.required)
    });

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'description', type: 'text', label: 'Description' }
        ]
    }));
}
