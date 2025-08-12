import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextFragmentForm } from './text-fragment-form';

describe('TextFragmentForm', () => {
    let component: TextFragmentForm;
    let fixture: ComponentFixture<TextFragmentForm>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TextFragmentForm]
        })
            .compileComponents();

        fixture = TestBed.createComponent(TextFragmentForm);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
