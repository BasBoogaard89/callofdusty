import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { RoomAdapterService } from '@infrastructure/adapters/room-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { RoomDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-room-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './room-list.html'
})
export class RoomList extends BaseListController<RoomDto> {
    service = inject(RoomAdapterService);
    editRoutePrefix = 'room';
    
    config: BaseListConfig<RoomDto> = {
        items: this.items,
        columns: ['description', 'category.description'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
