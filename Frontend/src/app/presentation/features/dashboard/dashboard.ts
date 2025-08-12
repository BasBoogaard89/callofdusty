import { Component } from '@angular/core';
import { BasePageController } from '@infrastructure/controllers/base-page.controller';

@Component({
    selector: 'app-dashboard',
    standalone: true,
    imports: [],
    templateUrl: './dashboard.html'
})
export class Dashboard extends BasePageController {
    ngOnInit() {
        this.setDynamicTitle();
    }
}
