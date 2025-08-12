import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChoreList } from './chore-list';

describe('ChoreList', () => {
    let component: ChoreList;
    let fixture: ComponentFixture<ChoreList>;

    beforeEach(async () => {
        await TestBed
            .configureTestingModule({
                imports: [ChoreList]
            })
            .compileComponents();

        fixture = TestBed.createComponent(ChoreList);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
