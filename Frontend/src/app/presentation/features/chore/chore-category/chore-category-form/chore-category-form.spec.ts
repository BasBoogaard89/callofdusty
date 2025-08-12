import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoreCategoryForm } from './chore-category-form';

describe('ChoreCategoryForm', () => {
    let component: ChoreCategoryForm;
    let fixture: ComponentFixture<ChoreCategoryForm>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [ChoreCategoryForm]
        })
            .compileComponents();

        fixture = TestBed.createComponent(ChoreCategoryForm);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
