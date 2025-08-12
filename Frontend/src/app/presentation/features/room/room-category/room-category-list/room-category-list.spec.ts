import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomCategoryList } from './room-category-list';

describe('RoomCategoryList', () => {
    let component: RoomCategoryList;
    let fixture: ComponentFixture<RoomCategoryList>;

    beforeEach(async () => {
        await TestBed
            .configureTestingModule({
                imports: [RoomCategoryList]
            })
            .compileComponents();

        fixture = TestBed.createComponent(RoomCategoryList);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
