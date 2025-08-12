import { Injectable, signal } from '@angular/core';

@Injectable({
    providedIn: 'root'
})
export class TitleService {
    readonly title = signal<string>('');

    set(title: string) {
        this.title.set(title);
    }

    clear() {
        this.title.set('');
    }
}
