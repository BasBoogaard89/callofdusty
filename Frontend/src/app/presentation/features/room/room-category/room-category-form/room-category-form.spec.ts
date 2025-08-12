import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomCategoryForm } from './room-category-form';

describe('RoomCategoryForm', () => {
    let component: RoomCategoryForm;
    let fixture: ComponentFixture<RoomCategoryForm>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [RoomCategoryForm]
        })
            .compileComponents();

        fixture = TestBed.createComponent(RoomCategoryForm);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
