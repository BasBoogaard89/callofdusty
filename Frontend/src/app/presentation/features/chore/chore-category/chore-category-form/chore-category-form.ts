import { Component, computed, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { ChoreCategoryDto } from '@generated/api';
import { ChoreCategoryAdapterService } from '@infrastructure/adapters/chore-category-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';

@Component({
    selector: 'app-chore-category-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './chore-category-form.html'
})
export class ChoreCategoryForm extends BaseFormController<ChoreCategoryDto> {
    service = inject(ChoreCategoryAdapterService);
    listRoutePrefix = 'chore/category';

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
