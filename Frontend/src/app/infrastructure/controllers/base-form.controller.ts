import { Component, inject, signal } from "@angular/core";
import { FormGroup } from "@angular/forms";
import { BaseCrudService } from "@infrastructure/services/base-crud.service";
import { BasePageController } from "./base-page.controller";
import { MatDialog, MatDialogRef } from "@angular/material/dialog";
import { firstValueFrom } from "rxjs";

@Component({
    template: ''
})
export abstract class BaseFormController<T extends { id?: number }> extends BasePageController {
    abstract listRoutePrefix: string;
    abstract form: FormGroup;
    abstract service: BaseCrudService<T>;

    dialog = inject(MatDialog);
    protected dialogRef?: MatDialogRef<any> = inject(MatDialogRef, { optional: true });

    model = signal<T | null>(null);

    async ngOnInit() {
        const id = Number(this.route.snapshot.paramMap.get('id'));
        this.model.set(id !== 0 ? await this.service.getById(id!) : null);

        this.form.patchValue(this.model());
        this.setDynamicTitle(this.model());

        await this.loadAdditionalData();
    }

    async save() {
        this.model.set({
            ...this.model(),
            ...this.form.getRawValue()
        } as T);

        var value = await this.service.save(this.model()!);
        
        if (this.dialogRef) {
            this.dialogRef.close(value);
        } else {
            this.router.navigateByUrl(this.getListRoute());
        }
    }

    getListRoute(): string {
        return `/${this.listRoutePrefix}`;
    }

    protected async loadAdditionalData(): Promise<void> {
    }
    
    async openCreateDialog(component: any) {
        const ref = this.dialog.open(component, { autoFocus: 'first-tabbable' });
        const created = await firstValueFrom(ref.afterClosed());
        
        if (created?.id) {
            this.loadAdditionalData();
            return { value: created.id, label: created.description };
        }

        return null;
    }
}
