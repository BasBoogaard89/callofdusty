import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MATERIAL_IMPORTS } from '@infrastructure/material.imports';

@Component({
    selector: 'app-menu',
    imports: [
        CommonModule,
        RouterModule,
        ...MATERIAL_IMPORTS
    ],
    templateUrl: './menu.html'
})
export class Menu {
    router: Router = inject(Router);

    nav = [
        { icon: 'space_dashboard', label: 'Dashboard', link: '/dashboard' },
        { icon: 'play_arrow', label: 'Start new quest', link: '/quest' },
        {
            icon: 'checklist', label: 'Chores', children: [
                { label: 'Chores', link: '/chore' },
                { label: 'Chore categories', link: '/chore/category' },
            ]
        },
        {
            icon: 'grid_view', label: 'Rooms', children: [
                { label: 'Rooms', link: '/room' },
                { label: 'Room categories', link: '/room/category' },
            ]
        },
        {
            icon: 'palette', label: 'Themes', children: [
                { label: 'Themes', link: '/theme' },
                { label: 'Text templates', link: '/theme/text-template' },
                { label: 'Text fragments', link: '/theme/text-fragment' },
            ]
        },
        { icon: 'history', label: 'History', link: '/history' },
    ];

    go(ev: any, link?: string) {
        if (link) {
            this.router.navigateByUrl(link);
        }
    }

}
