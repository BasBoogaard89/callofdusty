import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextFragmentList } from './text-fragment-list';

describe('TextFragmentList', () => {
    let component: TextFragmentList;
    let fixture: ComponentFixture<TextFragmentList>;

    beforeEach(async () => {
        await TestBed
            .configureTestingModule({
                imports: [TextFragmentList]
            })
            .compileComponents();

        fixture = TestBed.createComponent(TextFragmentList);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
