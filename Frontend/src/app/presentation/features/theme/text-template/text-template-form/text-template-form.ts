import { Component, computed, inject, signal } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { DynamicFormSchema } from '@infrastructure/interfaces/dynamic-form.config';
import { TextTemplateDto, ChoreCategoryDto, RoomCategoryDto, ThemeDto, CategoryType } from '@generated/api';
import { TextTemplateAdapterService } from '@infrastructure/adapters/text-template-adapter.service';
import { BaseFormController } from '@infrastructure/controllers/base-form.controller';
import { BaseForm } from '@presentation/base/base-form/base-form';
import { ChoreCategoryAdapterService } from '@infrastructure/adapters/chore-category-adapter.service';
import { RoomCategoryAdapterService } from '@infrastructure/adapters/room-category-adapter.service';
import { ThemeAdapterService } from '@infrastructure/adapters/theme-adapter.service';
import { ChoreCategoryForm } from '@presentation/features/chore/chore-category/chore-category-form/chore-category-form';
import { RoomCategoryForm } from '@presentation/features/room/room-category/room-category-form/room-category-form';
import { ThemeForm } from '../../theme/theme-form/theme-form';

@Component({
    selector: 'app-text-template-form',
    standalone: true,
    imports: [BaseForm],
    templateUrl: './text-template-form.html'
})
export class TextTemplateForm extends BaseFormController<TextTemplateDto> {
    service = inject(TextTemplateAdapterService);
    choreCategoryService = inject(ChoreCategoryAdapterService);
    roomCategoryService = inject(RoomCategoryAdapterService);
    themeService = inject(ThemeAdapterService);

    listRoutePrefix = 'theme/text-template';

    form = new FormGroup({
        id: new FormControl(0),
        description: new FormControl('', Validators.required),
        categoryType: new FormControl('', Validators.required),
        categoryId: new FormControl(null, Validators.required),
        themeId: new FormControl(null, Validators.required),
    });
    
    choreCategories = signal<ChoreCategoryDto[]>([]);
    roomCategories = signal<RoomCategoryDto[]>([]);
    themes= signal<ThemeDto[]>([]);
    
    categoryOptions = Object.values(CategoryType);

    schema = computed<DynamicFormSchema>(() => ({
        fields: [
            { key: 'description', type: 'text', label: 'Description' },
            { key: 'categoryType', type: 'select', label: 'Category type', options: () => this.categoryOptions.map(e => ({ value: e, label: e })) },
            { key: 'categoryId', type: 'select', label: 'Category',
                showIf: ({ form }) => form.get('categoryType')?.value == 'Chore' || form.get('categoryType')?.value == 'Room',
                options: () => {
                    const type = this.form.get('categoryType')?.value;
                    if (type === 'Chore') {
                        return this.choreCategories().map(c => ({ value: c.id, label: c.description }));
                    } else {
                        return this.roomCategories().map(r => ({ value: r.id, label: r.description }));
                    }
                },
                create: () => {
                    const type = this.form.get('categoryType')?.value;
                    if (type === 'Chore') {
                        return this.openCreateDialog(ChoreCategoryForm);
                    } else {
                        return this.openCreateDialog(RoomCategoryForm);
                    }
                },
            },
            { key: 'themeId', type: 'select', label: 'Theme',
                options: () => this.themes().map(e => ({ value: e.id, label: e.description })),
                create: () => this.openCreateDialog(ThemeForm)
            }
        ]
    }));

    protected override async loadAdditionalData() {
        this.choreCategories.set(await this.choreCategoryService.getAll());
        this.roomCategories.set(await this.roomCategoryService.getAll());
        this.themes.set(await this.themeService.getAll());
    }
}
