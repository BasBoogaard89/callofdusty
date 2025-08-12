import { Component } from '@angular/core';
import { HistoryForm } from '../history-form/history-form';

@Component({
    selector: 'app-history-detail',
    standalone: true,
    imports: [HistoryForm],
    templateUrl: './history-detail.html'
})
export class HistoryDetail {
}
