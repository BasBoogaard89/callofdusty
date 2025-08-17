import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { TextTemplateDto, TextTemplateService } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class TextTemplateAdapterService extends BaseCrudService<TextTemplateDto> {
    api = new TextTemplateService(new AxiosHttpRequest(this.apiConfig()));
    
    protected override get cacheKeys() {
        return ['text-template', 'chore-category', 'room-category', 'theme'] as const;
    }
    
    getAll(): Promise<TextTemplateDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getTextTemplate()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<TextTemplateDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getTextTemplate1(id);
        return result ?? undefined;
    }

    save(item: TextTemplateDto): Promise<TextTemplateDto> {
        return this.api.postTextTemplate(item).then((res) => {
            this.invalidate('text-template');
            
            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteTextTemplate(id).then(() => { this.invalidate('text-template'); });
    }
}
