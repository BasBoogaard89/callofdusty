import { Component } from '@angular/core';
import { RoomCategoryForm } from '../room-category-form/room-category-form';

@Component({
    selector: 'app-room-category-detail',
    standalone: true,
    imports: [RoomCategoryForm],
    templateUrl: './room-category-detail.html'
})
export class RoomCategoryDetail {
}
