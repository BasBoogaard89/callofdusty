import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextTemplateForm } from './text-template-form';

describe('TextTemplateForm', () => {
    let component: TextTemplateForm;
    let fixture: ComponentFixture<TextTemplateForm>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TextTemplateForm]
        })
            .compileComponents();

        fixture = TestBed.createComponent(TextTemplateForm);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
