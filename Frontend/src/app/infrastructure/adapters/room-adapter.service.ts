import { Injectable } from '@angular/core';
import { BaseCrudService } from '@infrastructure/services/base-crud.service';
import { RoomDto, RoomService } from '@generated/api';
import { AxiosHttpRequest } from '@generated/api/core/AxiosHttpRequest';

@Injectable({
    providedIn: 'root'
})
export class RoomAdapterService extends BaseCrudService<RoomDto> {
    api = new RoomService(new AxiosHttpRequest(this.apiConfig()));
    
    protected override get cacheKeys() {
        return ['room', 'room-category', 'chore'] as const;
    }
    
    getAll(): Promise<RoomDto[]> {
        if (this.isCacheValid()) {
            return Promise.resolve(this.cloneCache());
        }

        if (this.cachedData) {
            return this.cachedData;
        }

        this.cachedData = this.api.getRoom()
            .then(list => this.setCache(list))
            .finally(() => {
                this.cachedData = undefined;
            });

        return this.cachedData;
    }

    getById(id: number): Promise<RoomDto | undefined> {
        if (this.isCacheValid()) {
            const hit = this.cache!.find(x => (x as any).id === id);
            if (hit) return Promise.resolve(hit);
        }
        
        const result = this.api.getRoom1(id);
        return result ?? undefined;
    }

    save(item: RoomDto): Promise<RoomDto> {
        return this.api.postRoom(item).then((res) => {
            this.invalidate('room');
            
            return res;
        });
    }

    delete(id: number): Promise<void> {
        return this.api.deleteRoom(id).then(() => { this.invalidate('room'); });
    }
}
