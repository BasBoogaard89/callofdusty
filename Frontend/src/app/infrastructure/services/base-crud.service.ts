import { inject } from "@angular/core";
import { CacheInvalidationService } from "@infrastructure/services/cache-invalidation.service";

export abstract class BaseCrudService<T> {
    protected cacheService = inject(CacheInvalidationService);

    protected cache: T[] | null = null;
    protected cacheTs = 0;
    protected readonly ttlMs = 60_000;
    protected cachedData?: Promise<T[]>;
    protected get cacheKeys(): readonly string[] { return []; } 

    constructor() {
        const keys = this.cacheKeys;
        if (keys.length) {
            this.cacheService.on(keys as string[]).subscribe(() => this.invalidateCache());
        }
    }

    protected invalidateCache() {
        this.cache = null;
        this.cacheTs = 0;
    }

    protected isCacheValid(): boolean {
        return !!this.cache && (this.ttlMs <= 0 || (Date.now() - this.cacheTs) < this.ttlMs);
    }

    protected setCache(list: T[] = []): T[] {
        this.cache = list;
        this.cacheTs = Date.now();
        return this.cloneCache();
    }

    protected cloneCache(): T[] {
        return (this.cache ?? []).slice();
    }

    protected invalidate(type: string){
        this.cacheService.invalidate(type);
    }

    abstract getAll(): Promise<T[]>;
    abstract getById(id: number): Promise<T | undefined>;
    abstract save(item: T): Promise<T>;
    abstract delete(id: number): Promise<void>;
}