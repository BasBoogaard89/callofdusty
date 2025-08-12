import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextFragmentDetail } from './text-fragment-detail';

describe('TextFragmentDetail', () => {
    let component: TextFragmentDetail;
    let fixture: ComponentFixture<TextFragmentDetail>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TextFragmentDetail]
        })
            .compileComponents();

        fixture = TestBed.createComponent(TextFragmentDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
