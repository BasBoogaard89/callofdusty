import { inject } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { TitleService } from "@infrastructure/services/title.service";

export abstract class BasePageController {
    protected route = inject(ActivatedRoute);
    protected router = inject(Router);
    protected titleService = inject(TitleService);
    
    protected setDynamicTitle<T extends Record<string, any>>(data?: T) {
        const title = this.route.snapshot.data['title'] as string;
        if (!title) return;

        const rendered = title.replace(/\{\{(\w+)\}\}/g, (_, key) => {
            return data?.[key] ?? '';
        });
        
        this.titleService.set(rendered);
    }
}
