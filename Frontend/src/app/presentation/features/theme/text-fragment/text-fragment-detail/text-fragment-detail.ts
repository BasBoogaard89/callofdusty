import { Component } from '@angular/core';
import { TextFragmentForm } from '../text-fragment-form/text-fragment-form';

@Component({
    selector: 'app-text-fragment-detail',
    standalone: true,
    imports: [TextFragmentForm],
    templateUrl: './text-fragment-detail.html'
})
export class TextFragmentDetail {
}
