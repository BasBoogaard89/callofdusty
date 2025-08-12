import { Component, inject, signal } from "@angular/core";
import { Router } from "@angular/router";
import { BaseCrudService } from "@infrastructure/services/base-crud.service";
import { BasePageController } from "./base-page.controller";

@Component({
    template: ''
})
export abstract class BaseListController<T extends { id?: number }> extends BasePageController {
    abstract editRoutePrefix: string;
    abstract service: BaseCrudService<T>;
    
    items= signal<T[]>([]);

    ngOnInit() {
        this.refreshData();
        
        this.setDynamicTitle();
    }
    
    async refreshData() {
        this.items.set(await this.service.getAll());
    }

    onAdd = () => {
        this.router.navigateByUrl(this.getEditRoute(0));
    };

    onEdit = (item: T) => {
        this.router.navigateByUrl(this.getEditRoute(item.id));
    };

    onDelete = async (id: number) => { 
        await this.service.delete(id);
        this.refreshData();
    };
    
    getEditRoute(id: number): string {
         return `/${this.editRoutePrefix}/${id}`;
    }
}
