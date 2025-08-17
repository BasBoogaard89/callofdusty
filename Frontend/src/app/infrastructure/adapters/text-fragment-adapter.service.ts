import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { TextFragmentDto, TextFragmentService } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class TextFragmentAdapterService extends BaseCrudService<TextFragmentDto> {
    api = new TextFragmentService(new AxiosHttpRequest(this.apiConfig()));
    
    protected override get cacheKeys() {
        return ['text-fragment', 'text-template'] as const;
    }
    
    getAll(): Promise<TextFragmentDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getTextFragment()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<TextFragmentDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getTextFragment1(id);
        return result ?? undefined;
    }

    save(item: TextFragmentDto): Promise<TextFragmentDto> {
        return this.api.postTextFragment(item).then((res) => {
            this.invalidate('text-fragment');
            
            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteTextFragment(id).then(() => { this.invalidate('text-fragment'); });
    }
}
