import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'humanizeColumn', pure: true })
export class HumanizeColumnPipe implements PipeTransform {
    transform(value: string | null | undefined): string {
        if (!value) return '';
        // 1) dots/underscores/kebab -> spatie
        let s = value.replace(/[._-]+/g, ' ');
        // 2) camelCase → spatie
        s = s.replace(/([a-z0-9])([A-Z])/g, '$1 $2');
        // 3) dubbele spaties eruit
        s = s.replace(/\s+/g, ' ').trim();
        // 4) elk woord kapitaliseren (behoud acroniemen)
        s = s.split(' ')
            .map(w => this.capitalize(w))
            .join(' ');
        return s;
    }

    private capitalize(word: string): string {
        // Laat bestaande acroniemen (2+ hoofdletters) zo
        if (/^[A-Z]{2,}$/.test(word)) return word;
        return word.charAt(0).toUpperCase() + word.slice(1).toLowerCase();
    }
}
