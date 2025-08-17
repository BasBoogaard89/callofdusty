import { Routes } from '@angular/router';
import { authGuardChild } from '@infrastructure/guards/auth-guard';
import { guestGuard } from '@infrastructure/guards/guest-guard';

export const routes: Routes = [
    {
        path: 'login',
        data: { title: 'Login' },
        canActivate: [guestGuard],
        loadComponent: () =>
            import('@presentation/features/login/login').then(m => m.Login),
    },
    {
        path: '',
        canActivateChild: [authGuardChild],
        children: [
            {
                path: '',
                redirectTo: 'dashboard',
                pathMatch: 'full',
            },
            {
                path: 'dashboard',
                data: { title: 'Dashboard' },
                loadComponent: () =>
                    import('@presentation/features/dashboard/dashboard').then(m => m.Dashboard),
            },
            {
                path: 'chore',
                data: { title: 'Chores' },
                loadComponent: () =>
                    import('@presentation/features/chore/chore/chore-list/chore-list').then(m => m.ChoreList),
            },
            {
                path: 'chore/category',
                data: { title: 'Chore categories' },
                loadComponent: () =>
                    import('@presentation/features/chore/chore-category/chore-category-list/chore-category-list').then(m => m.ChoreCategoryList),
            },
            {
                path: 'chore/category/:id',
                data: { title: 'View chore category: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/chore/chore-category/chore-category-detail/chore-category-detail').then(m => m.ChoreCategoryDetail),
            },
            {
                path: 'chore/:id',
                data: { title: 'View chore: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/chore/chore/chore-detail/chore-detail').then(m => m.ChoreDetail),
            },
            {
                path: 'room',
                data: { title: 'Rooms' },
                loadComponent: () =>
                    import('@presentation/features/room/room/room-list/room-list').then(m => m.RoomList),
            },
            {
                path: 'room/category',
                data: { title: 'Room categories' },
                loadComponent: () =>
                    import('@presentation/features/room/room-category/room-category-list/room-category-list').then(m => m.RoomCategoryList),
            },
            {
                path: 'room/category/:id',
                data: { title: 'View room category: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/room/room-category/room-category-detail/room-category-detail').then(m => m.RoomCategoryDetail),
            },
            {
                path: 'room/:id',
                data: { title: 'View room: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/room/room/room-detail/room-detail').then(m => m.RoomDetail),
            },
            {
                path: 'theme',
                data: { title: 'Themes' },
                loadComponent: () =>
                    import('@presentation/features/theme/theme/theme-list/theme-list').then(m => m.ThemeList),
            },
            {
                path: 'theme/text-template',
                data: { title: 'Text template' },
                loadComponent: () =>
                    import('@presentation/features/theme/text-template/text-template-list/text-template-list').then(m => m.TextTemplateList),
            },
            {
                path: 'theme/text-template/:id',
                data: { title: 'View text template: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/theme/text-template/text-template-detail/text-template-detail').then(m => m.TextTemplateDetail),
            },
            {
                path: 'theme/text-fragment',
                data: { title: 'Text fragment' },
                loadComponent: () =>
                    import('@presentation/features/theme/text-fragment/text-fragment-list/text-fragment-list').then(m => m.TextFragmentList),
            },
            {
                path: 'theme/text-fragment/:id',
                data: { title: 'View text fragment: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/theme/text-fragment/text-fragment-detail/text-fragment-detail').then(m => m.TextFragmentDetail),
            },
            {
                path: 'theme/:id',
                data: { title: 'View theme: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/theme/theme/theme-detail/theme-detail').then(m => m.ThemeDetail),
            },
            {
                path: 'history',
                data: { title: 'History' },
                loadComponent: () =>
                    import('@presentation/features/history/history-list/history-list').then(m => m.HistoryList),
            },
            {
                path: 'history/:id',
                data: { title: 'View history record' },
                loadComponent: () =>
                    import('@presentation/features/history/history-detail/history-detail').then(m => m.HistoryDetail),
            },
            {
                path: 'complete/:choreId',
                data: { title: 'Complete chore: {{description}}' },
                loadComponent: () =>
                    import('@presentation/features/history/history-detail/history-detail').then(m => m.HistoryDetail),
            },
            {
                path: 'quest',
                data: { title: 'New quest' },
                loadComponent: () =>
                    import('@presentation/features/quest/quest').then(m => m.Quest),
            },
        ],
    },
];
