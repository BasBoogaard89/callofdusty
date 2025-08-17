import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { ThemeDto, ThemeService } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class ThemeAdapterService extends BaseCrudService<ThemeDto> {
    api = new ThemeService(new AxiosHttpRequest(this.apiConfig()));

    protected override get cacheKeys() {
        return ['theme', 'history', 'chore'] as const;
    }

    getAll(): Promise<ThemeDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getTheme()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<ThemeDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getTheme1(id);
        return result ?? undefined;
    }

    save(item: ThemeDto): Promise<ThemeDto> {
        return this.api.postTheme(item).then((res) => {
            this.invalidate('theme');

            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteTheme(id).then(() => { this.invalidate('theme'); });
    }
}
