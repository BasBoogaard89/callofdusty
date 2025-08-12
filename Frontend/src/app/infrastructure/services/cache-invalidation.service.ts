import { Injectable } from '@angular/core';
import { merge, Observable, Subject } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class CacheInvalidationService {
    private channels = new Map<string, Subject<void>>();

    invalidate(key: string) {
        if (!this.channels.has(key)) {
            this.channels.set(key, new Subject<void>());
        }

        this.channels.get(key)!.next();
    }

    on(keys: string[]): Observable<void> {
        const streams = keys.map(k => {
            if (!this.channels.has(k)) {
                this.channels.set(k, new Subject<void>());
            }
            
            return this.channels.get(k)!;
        });

        return merge(...streams);
    }
}
