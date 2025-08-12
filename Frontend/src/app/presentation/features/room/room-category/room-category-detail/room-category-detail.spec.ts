import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RoomCategoryDetail } from './room-category-detail';

describe('RoomCategoryDetail', () => {
    let component: RoomCategoryDetail;
    let fixture: ComponentFixture<RoomCategoryDetail>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [RoomCategoryDetail]
        })
            .compileComponents();

        fixture = TestBed.createComponent(RoomCategoryDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
