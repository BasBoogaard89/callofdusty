import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoreCategoryDetail } from './chore-category-detail';

describe('ChoreCategoryDetail', () => {
    let component: ChoreCategoryDetail;
    let fixture: ComponentFixture<ChoreCategoryDetail>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [ChoreCategoryDetail]
        })
            .compileComponents();

        fixture = TestBed.createComponent(ChoreCategoryDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
