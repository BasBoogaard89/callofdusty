import { Component, inject } from '@angular/core';
import { BaseListController } from '@infrastructure/controllers/base-list.controller';
import { RoomCategoryAdapterService } from '@infrastructure/adapters/room-category-adapter.service';
import { BaseList } from '@presentation/base/base-list/base-list';
import { RoomCategoryDto } from '@generated/api';
import { BaseListConfig } from '@infrastructure/interfaces/base-list-config';

@Component({
    selector: 'app-room-category-list',
    standalone: true,
    imports: [BaseList],
    templateUrl: './room-category-list.html'
})
export class RoomCategoryList extends BaseListController<RoomCategoryDto> {
    service = inject(RoomCategoryAdapterService);
    editRoutePrefix = 'room/category';
    
    config: BaseListConfig<RoomCategoryDto> = {
        items: this.items,
        columns: ['description'],
        onAdd: () => this.onAdd(),
        onEdit: (item) => this.onEdit(item),
        onDelete: (id) => this.onDelete(id)
    };
}
