import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { ChoreCategoryAdapterService } from '@infrastructure/adapters/chore-category-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { ChoreCategoryDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-chore-category-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './chore-category-list.html'
})
export class ChoreCategoryList extends BaseListController<ChoreCategoryDto> {
    service = inject(ChoreCategoryAdapterService);
    editRoutePrefix = 'chore/category';

    config: BaseListConfig<ChoreCategoryDto> = {
        items: this.items,
        columns: ['description'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
