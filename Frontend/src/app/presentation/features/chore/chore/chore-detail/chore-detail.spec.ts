import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoreDetail } from './chore-detail';

describe('ChoreDetail', () => {
    let component: ChoreDetail;
    let fixture: ComponentFixture<ChoreDetail>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [ChoreDetail]
        })
            .compileComponents();

        fixture = TestBed.createComponent(ChoreDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
