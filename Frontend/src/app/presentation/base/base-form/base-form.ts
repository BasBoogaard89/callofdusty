import { Component, input, output } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { DynamicFormSchema, FormField, Option } from '@infrastructure/interfaces/dynamic-form.config';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';

@Component({
    selector: 'app-base-form',
    standalone: true,
    imports: [
        ReactiveFormsModule,
        ...MATERIAL_IMPORTS
    ],
    providers: [
        {
            provide: MAT_FORM_FIELD_DEFAULT_OPTIONS,
            useValue: {
                appearance: 'outline'
            }
        }
    ],
    templateUrl: './base-form.html'
})
export class BaseForm {
    form = input.required<FormGroup>();
    schema = input.required<DynamicFormSchema>();
    data = input<any>();
    submitted = output<{ value: any; valid: boolean }>();

    private optionsCache = new Map<string, Option[]>();

    resolveOptions(f: FormField): Option[] {
        const v = typeof f.options === 'function' ? f.options() : (f.options ?? []);
        if (v instanceof Promise) {
            v.then(res => this.optionsCache.set(f.key, res ?? []));
            return this.optionsCache.get(f.key) ?? [];
        }
        this.optionsCache.set(f.key, v);
        return v;
    }

    async onCreateOption(f: FormField) {
        if (!f.create) return;
        const newOpt = await f.create();
        if (!newOpt) return;
        
        const cur = this.optionsCache.get(f.key) ?? [];
        this.optionsCache.set(f.key, [...cur, newOpt]);
        this.form().get(f.key)?.setValue(newOpt.value);
    }
}
