import { Component, inject } from '@angular/core';
import { BaseListController } from 'app/infrastructure/controllers/base-list.controller';
import { ChoreAdapterService } from 'app/infrastructure/adapters/chore-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { ChoreDto, DirtinessFactor } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-chore-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './chore-list.html'
})
export class ChoreList extends BaseListController<ChoreDto> {
    service = inject(ChoreAdapterService);
    editRoutePrefix = 'chore';

    config: BaseListConfig<ChoreDto> = {
        items: this.items,
        columns: ['description', 'category.description', 'room.description', 'durationMinutes', 'frequencyDays', 'dirtinessFactor'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id),
        customSort: {
            dirtinessFactor: v => Object.values(DirtinessFactor).indexOf(v)
        }
    }
}
