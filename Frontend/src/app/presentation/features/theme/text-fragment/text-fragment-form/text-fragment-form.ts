import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { TextFragmentDto, TextTemplateDto } from '@generated/api';
import { TextFragmentAdapterService } from '@infrastructure/adapters/text-fragment-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';
import { TextTemplateAdapterService } from '@infrastructure/adapters/text-template-adapter.service';
import { TextTemplateForm } from '../../text-template/text-template-form/text-template-form';

@Component({
    selector: 'app-text-fragment-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './text-fragment-form.html'
})
export class TextFragmentForm extends BaseFormController<TextFragmentDto> {
    service = inject(TextFragmentAdapterService);
    textTemplateService = inject(TextTemplateAdapterService);
    listRoutePrefix = 'theme/text-fragment';

    form = new FormGroup({
        id: new FormControl(0),
        key: new FormControl('', Validators.required),
        value: new FormControl('', Validators.required),
        textTemplateId: new FormControl(0, Validators.required),
    });
    
    textTemplates = signal<TextTemplateDto[]>([]);

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'key', type: 'text', label: 'Key' },
            { key: 'value', type: 'text', label: 'Value' },
            { key: 'textTemplateId', type: 'select', label: 'Category',
                options: () => this.textTemplates().map(e => ({ value: e.id, label: e.description })),
                create: async () => this.openCreateDialog(TextTemplateForm)
            },
        ]
    }));

    protected override async loadAdditionalData() {
        this.textTemplates.set(await this.textTemplateService.getAll());
    }
}
