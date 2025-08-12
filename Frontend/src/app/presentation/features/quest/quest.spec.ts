import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Quest } from './quest';

describe('Quest', () => {
    let component: Quest;
    let fixture: ComponentFixture<Quest>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [Quest]
        })
            .compileComponents();

        fixture = TestBed.createComponent(Quest);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
