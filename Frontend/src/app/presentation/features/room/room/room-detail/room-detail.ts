import { Component } from '@angular/core';
import { RoomForm } from '../room-form/room-form';

@Component({
    selector: 'app-room-detail',
    standalone: true,
    imports: [RoomForm],
    templateUrl: './room-detail.html'
})
export class RoomDetail {
}
