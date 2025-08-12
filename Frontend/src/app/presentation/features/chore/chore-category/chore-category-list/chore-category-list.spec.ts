import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoreCategoryList } from './chore-category-list';

describe('ChoreCategoryList', () => {
    let component: ChoreCategoryList;
    let fixture: ComponentFixture<ChoreCategoryList>;

    beforeEach(async () => {
        await TestBed
            .configureTestingModule({
                imports: [ChoreCategoryList]
            })
            .compileComponents();

        fixture = TestBed.createComponent(ChoreCategoryList);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
