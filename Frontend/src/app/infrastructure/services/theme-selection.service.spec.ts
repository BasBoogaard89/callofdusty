import { TestBed } from '@angular/core/testing';

import { ThemeSelectionService } from './theme-selection.service';

describe('ThemeSelectionService', () => {
    let service: ThemeSelectionService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(ThemeSelectionService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
