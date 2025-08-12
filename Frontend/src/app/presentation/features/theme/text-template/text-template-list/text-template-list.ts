import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { TextTemplateAdapterService } from '@infrastructure/adapters/text-template-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { TextTemplateDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-text-template-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './text-template-list.html'
})
export class TextTemplateList extends BaseListController<TextTemplateDto> {
    service = inject(TextTemplateAdapterService);
    editRoutePrefix = 'theme/text-template';

    config: BaseListConfig<TextTemplateDto> = {
        items: this.items,
        columns: ['description'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
