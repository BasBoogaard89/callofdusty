import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseList } from './base-list';

describe('BaseList', () => {
    let component: BaseList<any>;
    let fixture: ComponentFixture<BaseList<any>>;

    beforeEach(async () => {
        await TestBed
            .configureTestingModule({
                imports: [BaseList]
            })
            .compileComponents();

        fixture = TestBed.createComponent(BaseList);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
