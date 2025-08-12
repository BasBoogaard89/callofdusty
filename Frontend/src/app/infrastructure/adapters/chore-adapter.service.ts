import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { ChoreDto, ChoreFilterDto, ChoreQuestDto, ChoreService, OpenAPI } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class ChoreAdapterService extends BaseCrudService<ChoreDto> {
    api = new ChoreService(new AxiosHttpRequest(OpenAPI));

    protected override get cacheKeys() {
        return ['chore', 'chore-category', 'room'] as const;
    }

    getAll(): Promise<ChoreDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getChore()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<ChoreDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getChore1(id);
        return result ?? undefined;
    }

    save(item: ChoreDto): Promise<ChoreDto> {
        return this.api.postChore(item).then((res) => {
            this.invalidate('chore');

            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteChore(id).then(() => { this.invalidate('chore'); });
    }

    getAllFiltered(filter: ChoreFilterDto): Promise<ChoreDto[]> {
        return this.api.postChoreGetAllFiltered(filter);
    }

    getQuest(themeId: number): Promise<ChoreQuestDto> {
        return this.api.getChoreQuest(themeId);
    }

    getAllQuests(themeId: number): Promise<ChoreQuestDto[]> {
        return this.api.getChoreAllQuests(themeId);
    }
}
