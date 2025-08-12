import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TextTemplateList } from './text-template-list';

describe('TextTemplateList', () => {
    let component: TextTemplateList;
    let fixture: ComponentFixture<TextTemplateList>;

    beforeEach(async () => {
        await TestBed
            .configureTestingModule({
                imports: [TextTemplateList]
            })
            .compileComponents();

        fixture = TestBed.createComponent(TextTemplateList);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
