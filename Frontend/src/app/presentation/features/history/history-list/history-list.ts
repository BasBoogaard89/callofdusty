import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { HistoryAdapterService } from '@infrastructure/adapters/history-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { HistoryDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-history-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './history-list.html'
})
export class HistoryList extends BaseListController<HistoryDto> {
    service = inject(HistoryAdapterService);
    editRoutePrefix = 'history';
    
    config: BaseListConfig<HistoryDto> = {
        items: this.items,
        columns: ['dateStarted', 'dateCompleted', 'choreId'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
