import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ThemeForm } from './theme-form';

describe('ThemeForm', () => {
    let component: ThemeForm;
    let fixture: ComponentFixture<ThemeForm>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [ThemeForm]
        })
            .compileComponents();

        fixture = TestBed.createComponent(ThemeForm);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
