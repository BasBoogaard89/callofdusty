import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { TextFragmentAdapterService } from '@infrastructure/adapters/text-fragment-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { TextFragmentDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-text-fragment-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './text-fragment-list.html'
})
export class TextFragmentList extends BaseListController<TextFragmentDto> {
    service = inject(TextFragmentAdapterService);
    editRoutePrefix = 'theme/text-fragment';
    
    config: BaseListConfig<TextFragmentDto> = {
        items: this.items,
        columns: ['key', 'value'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
