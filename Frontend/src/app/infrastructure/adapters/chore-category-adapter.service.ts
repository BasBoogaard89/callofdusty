import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { ChoreCategoryDto, ChoreCategoryService, OpenAPI } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class ChoreCategoryAdapterService extends BaseCrudService<ChoreCategoryDto> {
    api = new ChoreCategoryService(new AxiosHttpRequest(OpenAPI));
    
    protected override get cacheKeys() {
        return ['chore-category'] as const;
    }
    
    getAll(): Promise<ChoreCategoryDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getChoreCategory()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<ChoreCategoryDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getChoreCategory1(id);
        return result ?? undefined;
    }

    save(item: ChoreCategoryDto): Promise<ChoreCategoryDto> {
        return this.api.postChoreCategory(item).then((res) => {
            this.invalidate('chore-category');

            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteChoreCategory(id).then(() => { this.invalidate('chore-category'); });
    }
}
