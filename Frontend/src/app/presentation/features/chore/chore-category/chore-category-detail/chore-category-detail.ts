import { Component } from '@angular/core';
import { ChoreCategoryForm } from '../chore-category-form/chore-category-form';

@Component({
    selector: 'app-chore-category-detail',
    standalone: true,
    imports: [ChoreCategoryForm],
    templateUrl: './chore-category-detail.html'
})
export class ChoreCategoryDetail {
}
