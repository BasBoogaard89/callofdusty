import { Component } from '@angular/core';
import { TextTemplateForm } from '../text-template-form/text-template-form';

@Component({
    selector: 'app-text-template-detail',
    standalone: true,
    imports: [TextTemplateForm],
    templateUrl: './text-template-detail.html'
})
export class TextTemplateDetail {
}


