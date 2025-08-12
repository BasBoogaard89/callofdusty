import { Component } from '@angular/core';
import { ChoreForm } from "../chore-form/chore-form";

@Component({
    selector: 'app-chore-detail',
    standalone: true,
    imports: [ChoreForm],
    templateUrl: './chore-detail.html'
})
export class ChoreDetail {
}
