import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { RoomCategoryDto, RoomCategoryService, OpenAPI } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class RoomCategoryAdapterService extends BaseCrudService<RoomCategoryDto> {
    api = new RoomCategoryService(new AxiosHttpRequest(OpenAPI));
    
    protected override get cacheKeys() {
        return ['room-category'] as const;
    }
    
    getAll(): Promise<RoomCategoryDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getRoomCategory()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<RoomCategoryDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }

        const result = this.api.getRoomCategory1(id);
        return result ?? undefined;
    }

    save(item: RoomCategoryDto): Promise<RoomCategoryDto> {
        return this.api.postRoomCategory(item).then((res) => {
            this.invalidate('room-category');
            
            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteRoomCategory(id).then(() => { this.invalidate('room-category'); });
    }
}
