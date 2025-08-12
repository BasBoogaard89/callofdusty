import { Component, inject } from '@angular/core';
import { RouterModule } from '@angular/router';
import { Header } from '@presentation/layout/header/header';
import { Menu } from '@presentation/layout/menu/menu';
import { TitleService } from '@infrastructure/services/title.service';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';

@Component({
    selector: 'app-layout',
    standalone: true,
    imports: [
        RouterModule,
        Header,
        Menu,
        ...MATERIAL_IMPORTS
    ],
    templateUrl: './layout.html'
})
export class Layout {
    readonly titleService = inject(TitleService);
}
