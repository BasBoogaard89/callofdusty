import { Component, signal } from '@angular/core';
import { Layout } from "@presentation/layout/layout/layout";

@Component({
    selector: 'app-root',
    imports: [Layout],
    templateUrl: './app.html'
})
export class App {
    protected readonly title = signal('CallOfDusty');
}
