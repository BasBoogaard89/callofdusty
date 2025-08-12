import { Component, computed, inject } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { ThemeDto } from '@generated/api';
import { ThemeAdapterService } from '@infrastructure/adapters/theme-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';

@Component({
    selector: 'app-theme-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './theme-form.html'
})
export class ThemeForm extends BaseFormController<ThemeDto> {
    service = inject(ThemeAdapterService);
    listRoutePrefix = 'theme';

    form = new FormGroup({
        id: new FormControl(0),
        description: new FormControl('', Validators.required),
    });

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'description', type: 'text', label: 'Description' }
        ]
    }));
}
