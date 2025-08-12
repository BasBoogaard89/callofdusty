import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextTemplateDetail } from './text-template-detail';

describe('TextTemplateDetail', () => {
    let component: TextTemplateDetail;
    let fixture: ComponentFixture<TextTemplateDetail>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TextTemplateDetail]
        })
            .compileComponents();

        fixture = TestBed.createComponent(TextTemplateDetail);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
