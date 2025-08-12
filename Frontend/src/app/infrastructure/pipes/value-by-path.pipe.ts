import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'valueByPath', pure: true
})
export class ValueByPathPipe implements PipeTransform {
    transform(obj: any, path: string): any {
        console.log(1111);
        return path.split('.').reduce((acc, part) => acc?.[part], obj) ?? '';
    }
}
