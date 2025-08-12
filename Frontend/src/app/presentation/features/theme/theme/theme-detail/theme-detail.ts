import { Component } from '@angular/core';
import { ThemeForm } from '../theme-form/theme-form';

@Component({
    selector: 'app-theme-detail',
    standalone: true,
    imports: [ThemeForm],
    templateUrl: './theme-detail.html'
})
export class ThemeDetail {
}
