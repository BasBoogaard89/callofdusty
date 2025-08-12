import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { ThemeAdapterService } from '@infrastructure/adapters/theme-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { ThemeDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-theme-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './theme-list.html'
})
export class ThemeList extends BaseListController<ThemeDto> {
    service = inject(ThemeAdapterService);
    editRoutePrefix = 'theme';
    
    config: BaseListConfig<ThemeDto> = {
        items: this.items,
        columns: ['description'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
