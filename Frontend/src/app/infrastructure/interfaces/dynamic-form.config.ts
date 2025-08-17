import { FormGroup } from "@angular/forms";

type FieldType = 'text' | 'number' | 'select' | 'textarea' | 'checkbox' | 'date';
export type Option = { value: any; label: string };

export type FormField<T = any> = {
    key: keyof T & string;
    type: FieldType;
    label?: string;
    options?: Option[] | (() => Option[] | Promise<Option[]>);
    showIf?: (ctx: { form: FormGroup, data?: any }) => boolean;
    create?: () => Promise<Option | null>; 
    ui?: {
        hint?: string;
    };
};

export interface DynamicFormSchema<T = any> {
    fields: FormField<T>[];
}