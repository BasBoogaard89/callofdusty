import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { HistoryDto, HistoryService } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class HistoryAdapterService extends BaseCrudService<HistoryDto> {
    api = new HistoryService(new AxiosHttpRequest(this.apiConfig()));
    
    protected override get cacheKeys() {
        return ['history', 'chore'] as const;
    }
    
    getAll(): Promise<HistoryDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getHistory()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<HistoryDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getHistory1(id);
        return result ?? undefined;
    }

    save(item: HistoryDto): Promise<HistoryDto> {
        return this.api.postHistory(item).then((res) => {
            this.invalidate('history');

            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteHistory(id).then(() => { this.invalidate('history'); });
    }
}
